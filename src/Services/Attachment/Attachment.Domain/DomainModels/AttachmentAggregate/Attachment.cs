﻿using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public abstract class Attachment : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        private string _description;
        private string _notes;
        private readonly bool _isTagged;
        private readonly bool _isLayer3;
        private readonly int? _tenantId;
        public  AttachmentBandwidth AttachmentBandwidth { get; private set; }
        private readonly int _deviceId;
        private readonly Device _device;
        private readonly int _routingInstanceId;
        public RoutingInstance RoutingInstance { get; private set; }
        public ContractBandwidthPool ContractBandwidthPool { get; private set; }
        private readonly int _attachmentRoleId;
        private readonly AttachmentRole _attachmentRole;
        private readonly int _mtuId;
        public Mtu Mtu { get; private set; }
        private readonly List<Interface> _interfaces;
        public IReadOnlyCollection<Interface> Interfaces => _interfaces;
        private readonly List<Vif> _vifs;
        public IReadOnlyCollection<Vif> Vifs => _vifs;
        private bool _created;
        private readonly int _networkStatusId;

        protected Attachment()
        {
            _interfaces = new List<Interface>();
            _vifs = new List<Vif>();
            _created = true;
            _networkStatusId = NetworkStatus.Init.Id;
        }

        protected Attachment(string description, string notes, AttachmentBandwidth attachmentBandwidth, 
            AttachmentRole role, Mtu mtu, Device device, RoutingInstance routingInstance = null, 
            List<Ipv4AddressAndMask> ipv4Addresses = null, int? tenantId = null) : this()
        {
            this._device = device ?? throw new ArgumentNullException(nameof(device));
            this._deviceId = device.Id;
            this._attachmentRole = role ?? throw new ArgumentNullException(nameof(role));
            this._attachmentRoleId = role.Id;

            // The supplied attachment role must be compatible with the supplied device
            if (!device.DeviceRole.DeviceRoleAttachmentRoles
                                  .Select(d => d.AttachmentRole)
                                  .Contains(role))
            {
                throw new AttachmentDomainException($"Attachment role '{this._attachmentRole.Name}' is not valid for device '{device.Name}' ");
            }

            // Must have a tenant specified for a tenant-facing attachment
            if (role.IsTenantFacing)
            {
                if (!_tenantId.HasValue)
                {
                    throw new ArgumentNullException(nameof(tenantId));
                }

                _tenantId = tenantId;
            }

            Name = Guid.NewGuid().ToString("N");
            this.AttachmentBandwidth = attachmentBandwidth ?? throw new ArgumentNullException(nameof(attachmentBandwidth));
            this._description = description ?? throw new ArgumentNullException(nameof(description));
            this._notes = notes;
            this._isLayer3 = role.IsLayer3Role;
            this._isTagged = role.IsTaggedRole;
            this._created = true;
            this.Mtu = mtu ?? throw new ArgumentNullException(nameof(mtu));
            this._mtuId = mtu.Id;

            if (role.RequireRoutingInstance)
            {
                if (routingInstance != null)
                {
                    // Check the supplied routing instance is of a valid type
                    CheckRoutingInstanceType(routingInstance.RoutingInstanceType);

                    // It must be good!
                    this.RoutingInstance = routingInstance;
                    this._routingInstanceId = routingInstance.Id;
                }
                else
                {
                    // Create a new routing instance
                    this.RoutingInstance = CreateRoutingInstance(role.RoutingInstanceType, this._tenantId);
                }
            }
        }

        public void SetDescription(string description) => this._description = description;
        public void SetNotes(string notes) => this._notes = notes;
        public void ClearCreated() => this._created = false;
        public void SetMtu(Mtu mtu) => this.Mtu = mtu;
        public List<ContractBandwidthPool> GetContractBandwidthPools() => this.Vifs.Select(vif => vif.ContractBandwidthPool).ToList();

        /// <summary>
        /// Adds a vif to the attachment.
        /// </summary>
        /// <param name="isLayer3">If set to <c>true</c> the vif is enabled for layer3.</param>
        /// <param name="role">Role of the vif</param>
        /// <param name="vlanTagRange">Vlan tag range for assignment of a vlan tag to the vif</param>
        /// <param name="mtu">Maximum transmission unit for the vif </param>
        /// <param name="routingInstance">An existing routing instance to allocate to the vif</param>
        /// <param name="contractBandwidthPool">An existing contract bandwidth pool to allocate to the vif</param>
        /// <param name="contractBandwidth">Contract bandwidth denoting the bandwidth requested for the vif</param>
        /// <param name="ipv4Addresses">Ipv4 addresses to be assigned to the layer 3 vif.</param>
        /// <param name="tenantId">Tenant identifier denoting the tenant for which the vif is assigned</param>
        /// <param name="vlanTag">Vlan tag to be assigned to the vif. It not provided one will be assigned.</param>
        public void AddVif(bool isLayer3, VifRole role, VlanTagRange vlanTagRange, Mtu mtu,
            RoutingInstance routingInstance, ContractBandwidthPool contractBandwidthPool, ContractBandwidth contractBandwidth,
            List<Ipv4AddressAndMask> ipv4Addresses, bool trustReceivedCosAndDscp = false, int? tenantId = null, int? vlanTag = null)
        {
            // The supplied vif role must be compatible with the attachment
            if (!this._attachmentRole.VifRoles.Contains(role))
            {
                throw new AttachmentDomainException($"Vif role '{role.Name}' is not valid for attachment '{this.Name}' ");
            }

            if (!this._isTagged)
            {
                throw new AttachmentDomainException($"A Vif cannot be created for attachment '{this.Name}' because the attachment is not enabled for tagging. ");
            }

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
                vlanTag = AssignVlanTag(vlanTagRange);
            }

            var vlans = CreateVlans(ipv4Addresses);

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
                var availableBandwidthMbps = this.AttachmentBandwidth.BandwidthGbps * 1000 - usedBandwidthMbps;
                if (availableBandwidthMbps < contractBandwidth.BandwidthMbps)
                {
                    throw new AttachmentDomainException($"Insufficient bandwidth remaining. Attachment '{this.Name}' has '{availableBandwidthMbps}' " +
                    	"Mbps available.");
                }

                contractBandwidthPool = new ContractBandwidthPool(contractBandwidth, trustReceivedCosAndDscp, tenantId);
            }

            if (role.RequireRoutingInstance)
            {
                if (routingInstance != null)
                {
                    CheckRoutingInstanceType(routingInstance.RoutingInstanceType);
                }
                else
                {
                    CreateRoutingInstance(role.RoutingInstanceType, tenantId);
                }
            }

            var vif = new Vif(isLayer3, role, mtu, vlans, vlanTag.Value, routingInstance, 
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
        /// Assign ports from the device to the attachment.
        /// </summary>
        /// <returns>The ports.</returns>
        /// <param name="numPortsRequired">Number ports required.</param>
        /// <param name="portBandwidthRequiredGbps">Port bandwidth required gbps.</param>
        protected internal virtual List<Port> AssignPorts(int numPortsRequired, int portBandwidthRequiredGbps)
        {
            if (numPortsRequired <= 0) throw new AttachmentDomainException("Number of ports required for the attachment must be greater than 0.");

            // Get free ports from the device which belong to the required port pool and are of the required port bandwidth
            List<Port> ports = this._device.Ports.Where(
                                   port =>
                                   port.GetPortStatus() == PortStatus.Free &&
                                   port.GetPortBandwidthGbps() == portBandwidthRequiredGbps &&
                                   port.GetPortPoolId() == this._attachmentRole.PortPoolId)
                                   .Take(numPortsRequired)
                                   .ToList();

            // Check we have the required number of ports - the 'take' method will only return the number of ports found which may be 
            // less than the required number
            if (ports.Count() != numPortsRequired) throw new AttachmentDomainException("Could not find a sufficient number of free ports " +
                $"matching the requirements. {numPortsRequired} ports of {portBandwidthRequiredGbps} Gbps are required but {ports.Count()} free " +
                	"ports were found.");

            // Assign the ports
            ports.ForEach(port => port.Assign(this._tenantId));

            return ports;
        }

        /// <summary>
        /// Create interfaces for the attachment.
        /// </summary>
        /// <param name="ipv4Addresses">Ipv4 addresses.</param>
        /// <param name="ports">Ports.</param>
        protected internal virtual void CreateInterfaces(List<Ipv4AddressAndMask> ipv4Addresses, List<Port> ports)
        {
            var @interface = new Interface(ports: ports);

            if (_isLayer3)
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
        protected internal virtual List<Vlan> CreateVlans(List<Ipv4AddressAndMask> ipv4Addresses)
        {
            if (this._isLayer3)
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
            
                if (this._isLayer3)
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
        /// Release ports of the attachment back to inventory.
        /// </summary>
        protected internal virtual void ReleasePortsAsync()
        {
            this.GetPorts().ForEach(port => port.Release());
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
        /// Checks the type of the routing instance to ensure compatiibility with the role of the attachment.
        /// </summary>
        /// <param name="routingInstanceType">Routing instance type.</param>
        private void CheckRoutingInstanceType(RoutingInstanceType routingInstanceType)
        {
            if (this._attachmentRole.IsTenantFacing) 
            {
                if (routingInstanceType.Name != RoutingInstanceType.Vrf.Name)
                {
                    throw new AttachmentDomainException($"Routing instance type '{routingInstanceType.Name}' " +
                    	"cannot be used to create a routing instance for a tenant-facing attachment.");
                }
            }
        }

        /// <summary>
        /// Create a routing instance for an attachment or a vif.
        /// </summary>
        /// <returns>The routing instance.</returns>
        /// <param name="routingInstanceType">Routing instance type.</param>
        /// <param name="tenantId">Tenant identifier.</param>
        private RoutingInstance CreateRoutingInstance(RoutingInstanceType routingInstanceType, int? tenantId = null)
        {
            var routingInstance = new RoutingInstance(device: this._device, type: routingInstanceType, tenantId: tenantId);
            this._device.AddRoutingInstance(routingInstance);

            return routingInstance;
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