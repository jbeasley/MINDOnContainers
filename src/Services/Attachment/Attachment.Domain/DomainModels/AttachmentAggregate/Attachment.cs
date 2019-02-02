using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Attachment : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        private readonly string _description;
        private readonly string _notes;
        private readonly bool _isTagged;
        private readonly bool _isLayer3;
        private readonly int? _tenantId;
        private readonly AttachmentBandwidth _attachmentBandwidth;
        private readonly Device _device;
        private readonly RoutingInstance _routingInstance;
        private readonly List<ContractBandwidthPool> _contractBandwidthPools;
        private readonly AttachmentRole _attachmentRole;
        private readonly Mtu _mtu;
        private readonly List<Interface> _interfaces;
        private readonly List<Vif> _vifs;
        private readonly  bool _created;
        private readonly NetworkStatus _networkStatus;

        protected Attachment()
        {
            _contractBandwidthPools = new List<ContractBandwidthPool>();
            _interfaces = new List<Interface>();
            _vifs = new List<Vif>();
        }

        public Attachment(string description, string notes, AttachmentBandwidth attachmentBandwidth, RoutingInstance routingInstance, 
        AttachmentRole role, Mtu mtu, Device device, List<Ipv4AddressAndMask> ipv4Addresses, int? tenantId = null) : this()
        {
            // The supplied attachment role must be compatible with the supplied device
            if (!device.DeviceRole.DeviceRoleAttachmentRoles.Contains(attachmentRole))
            {
                throw new AttachmentDomainException($"Attachment role '{attachmentRole.Name}' is not valid for device '{device.Name}' ");
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

            _name = Guid.NewGuid('N');
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _attachmentRole = attachmentRole ?? throw new ArgumentNullException(nameof(attachmentRole));
            _attachmentBandwidth = attachmentBandwidth ?? throw new ArgumentNullException(nameof(attachmentBandwidth));
            _description = description ?? throw new ArgumentNullException(nameof(description));
            _notes = notes;
            _isLayer3 = attachmentRole.IsLayer3Role;
            _isTagged = attachmentRole.IsTaggedRole;
            _created = true;
            _mtu = mtu ?? throw new ArgumentNullException(nameof(mtu));
            if (role.RequireRoutingInstance)
            {
                if (routingInstance != null)
                {
                    if (routingInstance.RoutingInstanceType)
                }
                else
                {
                    CreateRoutingInstance();
                }
            }
        }

        public void AddVif(bool isLayer3, VifRole role, VlanTagRange vlanTagRange, Mtu mtu,
            RoutingInstance routingInstance, ContractBandwidthPool contractBandwidthPool,
            List<Ipv4AddressAndMask> ipv4Addresses, int? tenantId, int? vlanTag = null)
        {
            if (!this._isTagged)
            {
                throw new AttachmentDomainException($"A Vif cannot be created for attachment '{this.Name}' because the attachment is not enabled for tagging. ");
            }

            if (vlanTag.HasValue)
            {
                // Validate that the supplied vlan tag is unique
                if (this.Vifs.Select(vif => vif.VlanTag).Contains(vlanTag.Value))
                {
                    throw new AttachmentDomainException($"Vlan tag '{vlanTag}' is already used.");
                }
            }

            if (contractBandwidthPool != null)
            {
                // Validate the contract bandwidth pool belongs to this attachment
                if (!this._contractBandwidthPools.Contains(p => contractBandwidthPool == p))
                {
                    throw new AttachmentDomainException($"The supplied contract bandwidth pool does not exist or does not belong to attachment '{this.Name}'.");
                }
            }

            if (routingInstance != null)
            {
                // Validate the routing instance belongs to the device for this attachment
                if (!this._device.RoutingInstances.Contains(r => routingInstance == r))
                {
                    throw new AttachmentDomainException($"The supplied routing instance does not exist or does not belong to attachment '{this.Name}'.");
                }
            }

            var vif = new Vif(isLayer3, vlanTagRange, mtu, routingInstance, contractBandwidthPool, ipv4Addresses, tenantId, vlanTag);

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

        protected internal virtual List<Port> AssignPorts(int numPortRequired, int portBandwidthRequiredGbps)
        {
            List<Port> ports = this._device.Ports.Where(
                                   port =>
                                   port.GetPortStatus() == PortStatus.Free &&
                                   port.GetPortBandwidth() == portBandwidthRequiredGbps &&
                                   port.GetPortPoolId() == this.AttachmentRole.PortPoolId)
                                   .Take(numPortsRequired);

            // Check we have the required number of ports - the 'take' method will only return the number of ports found which may be 
            // less than the required number
            if (ports.Count() != numPortsRequired) throw new AttachmentDomainException("Could not find a sufficient number of free ports " +
                $"matching the requirements. {numPortsRequired} ports of {portBandwidthRequiredGbps} Gbps are required but {ports.Count()} free ports were found.");

            ports.ForEach(port => port.Assign(this._tenantId);
            return ports;
        }

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
        /// Release ports of the attachment back to inventory.
        /// </summary>
        protected internal virtual void ReleasePortsAsync()
        {
            var ports = this.Interfaces.SelectMany(@interface => @interface.Ports)
                                                   .ToList()
                                                   .ForEach(port => port.Release());
        }

        protected CreateRoutingInstance(RoutingInstanceType routingInstanceType)
        {
            var routingInstance = new RoutingInstance(device: this._device, routingInstanceType)
        }              
    }
}