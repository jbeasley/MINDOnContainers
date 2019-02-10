using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentBandwidthAggregate;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public abstract class Attachment : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        private string _description;
        private string _notes;
        private readonly string _locationName;
        private readonly string _planeName;
        private int? _tenantId;
        protected readonly int _attachmentBandwidthId;    
        private int _deviceId;
        private int _routingInstanceId;
        public RoutingInstance RoutingInstance { get; private set; }
        public ContractBandwidthPool ContractBandwidthPool { get; private set; }
        private readonly int _attachmentRoleId;
        private readonly int _mtuId;
        public Mtu Mtu { get; private set; }
        private readonly List<Interface> _interfaces;
        public IReadOnlyCollection<Interface> Interfaces => _interfaces;
        private readonly List<Vif> _vifs;
        public IReadOnlyCollection<Vif> Vifs => _vifs;
        private readonly int _attachmentStatusId;
        private readonly int _networkStatusId;
        public AttachmentStatus AttachmentStatus { get; protected set; }

        protected Attachment()
        {
            _interfaces = new List<Interface>();
            _vifs = new List<Vif>();
            _attachmentStatusId = AttachmentStatus.Initialised.Id;
            AttachmentStatus = AttachmentStatus.Initialised;
            _networkStatusId = NetworkStatus.Init.Id;
        }

        protected Attachment(string locationName, string description, string notes, AttachmentBandwidth attachmentBandwidth, 
            AttachmentRole role, bool enableJumboMtu, string planeName = null, int? tenantId = null) : this()
        {
            this._attachmentRoleId = role.Id;

            this._locationName = locationName;
            this.Name = Guid.NewGuid().ToString("N");
            this._attachmentBandwidthId = attachmentBandwidth?.Id ?? throw new ArgumentNullException(nameof(attachmentBandwidth));
            this._attachmentRoleId = role?.Id ?? throw new ArgumentNullException(nameof(role));
            this._description = description ?? throw new ArgumentNullException(nameof(description));
            this._notes = notes;
            _mtuId = enableJumboMtu ? Mtu.m9000.Id : Mtu.m1500.Id;
            this._planeName = planeName;

            SetTenantId(role, tenantId);
        }

        public void SetDescription(string description) => this._description = description;
        public void SetNotes(string notes) => this._notes = notes;
        public void SetMtu(Mtu mtu) => this.Mtu = mtu;
        public List<ContractBandwidthPool> GetContractBandwidthPools() => this.Vifs.Select(vif => vif.ContractBandwidthPool).ToList();
        public void SetDeviceId(int deviceId) => _deviceId = deviceId;
        public string GetLocationName() => this._locationName;
        public string GetPlaneName() => this._planeName;

        public void SetAttachmentRoutingInstance(string name, int? administratorSubField, int? assignedNumberSubField)
        {      
            var routingInstance = new RoutingInstance(this._deviceId, name, administratorSubField, assignedNumberSubField);
            this.RoutingInstance = routingInstance;        
        }

        public RoutingInstance GetRoutingInstance() => this.RoutingInstance;

        public int GetAttachmentBandwidthId() => this._attachmentBandwidthId;

        protected void SetTenantId(AttachmentRole role, int? tenantId = null)
        {
            // Must have a tenant specified for a tenant-facing attachment
            if (role.IsTenantFacing)
            {
                if (!_tenantId.HasValue)
                {
                    throw new ArgumentNullException(nameof(tenantId));
                }

                this._tenantId = tenantId;
            }
        }

        public abstract void AddPorts(AttachmentBandwidth attachmentBandwidth, List<Port> ports);

        /// <summary>
        /// Adds a vif to the attachment.
        /// </summary>
        /// <param name="role">Role of the vif</param>
        /// <param name="vlanTagRange">Vlan tag range for assignment of a vlan tag to the vif</param>
        /// <param name="mtu">Maximum transmission unit for the vif </param>
        /// <param name="contractBandwidthPool">An existing contract bandwidth pool to allocate to the vif</param>
        /// <param name="contractBandwidth">Contract bandwidth denoting the bandwidth requested for the vif</param>
        /// <param name="ipv4Addresses">Ipv4 addresses to be assigned to the layer 3 vif.</param>
        /// <param name="tenantId">Tenant identifier denoting the tenant for which the vif is assigned</param>
        /// <param name="vlanTag">Vlan tag to be assigned to the vif. It not provided one will be assigned.</param>
        public void AddVif(VifRole role, VlanTagRange vlanTagRange, Mtu mtu,
            ContractBandwidthPool contractBandwidthPool, AttachmentBandwidth attachmentBandwidth, ContractBandwidth contractBandwidth,
            List<Ipv4AddressAndMask> ipv4Addresses, bool trustReceivedCosAndDscp = false, int? tenantId = null, int? vlanTag = null)
        {
            // The supplied vif role must be compatible with the attachment
            if (role.AttachmentRole.Id != this._attachmentRoleId)
            {
                throw new AttachmentDomainException($"Vif role '{role.Name}' is not valid for attachment '{this.Name}' ");
            }

            // Attachment must be 'tagged' in order to create a vif
            if (!role.AttachmentRole.IsTaggedRole)
            {
                throw new AttachmentDomainException($"A Vif cannot be created for attachment '{this.Name}' because the attachment is not enabled for tagging. ");
            }

            // Supplied attachment bandwidth must match that set for the attachment
            if (attachmentBandwidth.Id != this._attachmentBandwidthId)
            {
                throw new AttachmentDomainException($"Attachment bandwidth '{attachmentBandwidth.BandwidthGbps}' is not valid for attachment '{this.Name}' ");
            }

            // If a vlan tag is supplied, validate that it is in the correct range 
            if (vlanTag.HasValue)
            {
                if (vlanTag < 2 || vlanTag > 4094)
                {
                    throw new AttachmentDomainException("The vlan tag must be between 2 and 4094.");
                }

                // Validate that the supplied vlan tag is unique
                if (this._vifs.Select(v => v.VlanTag).Contains(vlanTag.Value))
                {
                    throw new AttachmentDomainException($"Vlan tag '{vlanTag}' is already used.");
                }
            }
            else
            {
                // Assign a free vlan tag from the supplied range
                vlanTag = AssignVlanTag(vlanTagRange);
            }

            var vlans = CreateVlans(role.AttachmentRole, ipv4Addresses);

            if (contractBandwidthPool == null && contractBandwidth == null)
            {
                throw new AttachmentDomainException("Either a contract bandwidth or an existing contract bandwidth pool is needed to create a vif.");
            }

            if (contractBandwidthPool != null)
            {
                // Validate the contract bandwidth pool belongs to this attachment
                if (!this.GetContractBandwidthPools().Contains(contractBandwidthPool))
                {
                    throw new AttachmentDomainException($"The supplied contract bandwidth pool does not exist or does not belong to " +
                        "attachment '{this.Name}'.");
                }
            }

            if (contractBandwidth != null)
            {
                // Validate there is sufficient attachment bandwidth available
                var usedBandwidthMbps = this._vifs.Select(v => v.ContractBandwidthPool.ContractBandwidth.BandwidthMbps).Sum();
                var availableBandwidthMbps = attachmentBandwidth.BandwidthGbps * 1000 - usedBandwidthMbps;
                if (availableBandwidthMbps < contractBandwidth.BandwidthMbps)
                {
                    throw new AttachmentDomainException($"Insufficient bandwidth remaining. Attachment '{this.Name}' has '{availableBandwidthMbps}' " +
                        "Mbps available.");
                }

                contractBandwidthPool = new ContractBandwidthPool(contractBandwidth, trustReceivedCosAndDscp, tenantId);
            }

            var vif = new Vif(this.Id, role, mtu, vlans, vlanTag.Value, 
                contractBandwidthPool, tenantId, trustReceivedCosAndDscp);
                
            this._vifs.Add(vif);
        }

        /// <summary>
        /// Delete a vifs which belongs to the attachment
        /// </summary>
        /// <returns>An awaitable task</returns>
        public void DeleteVif(Vif vif)
        {
            // Additional logic before deleting a vif 
            this._vifs.Remove(vif);
        }

        /// <summary>
        /// Create interfaces for the attachment.
        /// </summary>
        /// <param name="ipv4Addresses">Ipv4 addresses.</param>
        protected internal virtual void CreateInterfaces(AttachmentRole role, List<Ipv4AddressAndMask> ipv4Addresses)
        {
            var @interface = new Interface();

            if (role.IsLayer3Role)
            {
                @interface.SetIpv4Address(ipv4Addresses.FirstOrDefault());
            }

            this._interfaces.Add(@interface);
        }

        /// <summary>
        /// Create vlans for a vif.
        /// </summary>
        /// <returns>The vlans.</returns>
        /// <param name="ipv4Addresses">Ipv4 addresses.</param>
        protected internal virtual List<Vlan> CreateVlans(AttachmentRole role, List<Ipv4AddressAndMask> ipv4Addresses)
        {
            if (role.IsLayer3Role)
            {
                if (ipv4Addresses.Count != this._interfaces.Count)
                {
                    throw new AttachmentDomainException($"Insufficient IPv4 addresses provided to create vlans for a new vif for attachment '{this.Name}'. " +
                        $"{ipv4Addresses.Count} were supplied but {this._interfaces.Count} are required.");
                }
            }

            var vlans = new List<Vlan>();
            foreach (var @interface in this._interfaces) 
            {
                var vlan = new Vlan();
            
                if (role.IsLayer3Role)
                {
                    var ipv4Address = ipv4Addresses.First();
                    vlan.SetIpv4Address(ipv4Address);
                    ipv4Addresses.Remove(ipv4Address);
                }

                @interface.AddVlan(vlan);
                vlans.Add(vlan);
            }

            return vlans;
        }

        /// <summary>
        /// Gets the ports assigned to the attachment.
        /// </summary>
        /// <returns>The ports.</returns>
        protected List<Port> GetPorts()
        {
            return this._interfaces.SelectMany(@interface => @interface.Ports).ToList();
        }

        /// <summary>
        /// Assigns a vlan tag to a vif.
        /// </summary>
        /// <returns>The vlan tag.</returns>
        /// <param name="range">Vlan tag range from which to assign the vlan</param>
        private int AssignVlanTag(VlanTagRange range)
        {
            int? vlanTag = Enumerable.Range(range.StartValue, range.EndValue - range.StartValue)
                                     .Except(this._vifs.Select(vif => vif.VlanTag))
                                     .FirstOrDefault();

            if (!vlanTag.HasValue) throw new AttachmentDomainException($"Failed to allocate a free vlan tag from range '{range.Name}'. " +
                    "Please contact your administrator to report this issue, or try another range.");

            return vlanTag.Value;
        }
    }
}