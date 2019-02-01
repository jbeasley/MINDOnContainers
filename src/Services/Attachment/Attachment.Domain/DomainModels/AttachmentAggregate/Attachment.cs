using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Attachment : Entity, IAggregateRoot
    {
        // DDD Patterns comment
        // Using private fields, allowed since EF Core 1.1, is a much better encapsulation
        // aligned with DDD Aggregates and Domain Entities (instead of properties and property collections)
        private readonly string _description;
        private readonly string _notes;
        private readonly bool _isTagged;
        private readonly bool _isLayer3;
        private readonly int _attachmentBandwidthID;
        private readonly int? _tenantId;
        private readonly int _deviceId;
        private readonly Device _device;
        private readonly RoutingInstance _routingInstance;
        private readonly int? _contractBandwidthPoolId;
        private readonly int _attachmentRoleId;
        private readonly AttachmentRole _attachmentRole;
        private readonly int _mtuId;
        private readonly  bool _created;
        private readonly NetworkStatus _networkStatus;

        // DDD Patterns comment
        // Using a private collection field, better for DDD Aggregate's encapsulation
        // so Interfaces cannot be added from "outside the AggregateRoot" directly to the collection,
        // but only through the method Attachment.AddInterface() which includes behaviour.
        private readonly List<Interface> _interfaces;
        public IReadOnlyCollection<Interface> Interfaces => _interfaces;

        private readonly List<Vif> _vifs;
        public IReadOnlyCollection<Vif> _vifs => _vifs;

        public Attachment(string description, string notes, int attachmentBandwidthId, int? tenantId = null, int? routingInstanceId = null, 
        AttachmentRole attachmentRole, int mtuId, Device device, List<Ipv4AddressAndMask> ipv4Addresses)
        {
            if (!device.DeviceRole.DeviceRoleAttachmentRoles.Contains(attachmentRole))
            {
                throw new AttachmentDomainException($"Attachment role '{attachmentRole.Name}' is not valid for device '{device.Name}' ");
            }
            _description = description ?? throw new ArgumentNullException(nameof(description));
            _notes = notes;
            _tenantId = tenantId;
            _attachmentRole = attachmentRole;
            _attachmentBandwidthID = attachmentBandwidthId;
            _isLayer3 = attachmentRole.IsLayer3Role;
            _isTagged = attachmentRole.IsTaggedRole;
            _created = true;
        }

        protected internal virtual void CreateInterfaces()
        {

        }

        public void AddInterface(string ipv4Address = null, string ipv4SubnetMask = null)
        {
            if (_isLayer3)
            {
                // A layer 3 Attachment must have IP address and subnet supplied for each of its interfaces
                if (string.IsNullOrEmpty(ipv4Address))
                {
                    throw new AttachmentDomainException("An interface for a layer 3 attachment must be given an IPv4 address.");
                }

                if (string.IsNullOrEmpty(ipv4SubnetMask))
                {
                    throw new AttachmentDomainException("An interface for a layer 3 attachment must be given an IPv4 subnet mask.");
                }
            }
            else
            {   // A non-layer 3 Attachment must NOT have IP address and subnet supplied for any of its interfaces
                if (!string.IsNullOrEmpty(ipv4Address))
                {
                    throw new AttachmentDomainException("An interface for a layer 3 attachment must NOT be given an IPv4 address.");
                }

                if (!string.IsNullOrEmpty(ipv4SubnetMask))
                {
                    throw new AttachmentDomainException("An interface for a layer 3 attachment must NOT be given an IPv4 subnet mask.");
                }
            }

            this.AddInterface(new Interface(ipv4Address, ipv4SubnetMask));
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
        /// Release ports of the attachment back to inventory.
        /// </summary>
        /// <returns>An awaitable task</returns>
        protected internal virtual void ReleasePortsAsync()
        {
            var ports = this.Interfaces.SelectMany(@interface => @interface.Ports)
                                                   .ToList()
                                                   .ForEach(port => port.Release());
        }

        /// <summary>
        /// Validate the state of the attachment.
        /// </summary>
        public virtual void Validate()
        {
            if (this.Mtu == null) throw new IllegalStateException("An MTU is required for the attachment.");
            if (this.AttachmentBandwidth == null) throw new IllegalStateException("An attachment bandwidth is required for the attachment.");
            if (this.AttachmentRole == null) throw new IllegalStateException("An attachment role is required for the attachment.");
            if (this.Device == null) throw new IllegalStateException("A device is required for the attachment.");
            if (!this.Interfaces.Any()) throw new IllegalStateException("At least one interface is required for the attachment.");
            if (!this.Device.DeviceRole.DeviceRoleAttachmentRoles
                .Any(
                    x =>
                    x.AttachmentRoleID == this.AttachmentRole.AttachmentRoleID))
            {
                throw new IllegalStateException($"The attachment role of '{this.AttachmentRole.Name}' is not valid for device '{this.Device.Name}' because " +
                    $"the device is assigned to role '{this.Device.DeviceRole.Name}'.");
            }
            if (this.AttachmentRole.PortPool.PortRole.PortRoleType == PortRoleTypeEnum.TenantFacing && this.Tenant == null)
            {
                throw new IllegalStateException("A tenant association is required for the attachment in accordance with the attachment role of " +
                    $"'{this.AttachmentRole.Name}'.");
            }
            else if (this.AttachmentRole.PortPool.PortRole.PortRoleType == PortRoleTypeEnum.ProviderInfrastructure && this.Tenant != null)
            {
                throw new IllegalStateException("A tenant association exists for the attachment but is NOT required in accordance with the " +
                    $"attachment role of '{this.AttachmentRole.Name}'.");
            }

            if (this.RoutingInstance == null && this.AttachmentRole.RoutingInstanceTypeID.HasValue)
                throw new IllegalStateException("Illegal routing instance state. A routing instance for the attachment is required in accordance " +
                    $"with the requested attachment role of '{this.AttachmentRole.Name}' but was not found.");

            if (this.RoutingInstance != null && !this.AttachmentRole.RoutingInstanceTypeID.HasValue)
                throw new IllegalStateException("Illegal routing instance state. A routing instance for the attachment has been assigned but is " +
                    $"not required for an attachment with attachment role of '{this.AttachmentRole.Name}'.");

            if (this.RoutingInstance != null && this.AttachmentRole.RoutingInstanceType != null)
            {
                if (this.RoutingInstance.RoutingInstanceType.RoutingInstanceTypeID != this.AttachmentRole.RoutingInstanceTypeID)
                {
                    throw new IllegalStateException("Illegal routing instance state. The routing instance type for the attachment is different to that " +
                        $"required by the attachment role. The routing instance type required is '{this.AttachmentRole.RoutingInstanceType.Type.ToString()}'. " +
                        $"The routing instance type assigned to the attachment is '{this.RoutingInstance.RoutingInstanceType.Type.ToString()}'.");
                }
            }

            if (this.IsLayer3)
            {
                if (this.Interfaces.Count(x => !string.IsNullOrEmpty(x.IpAddress) &&
                !string.IsNullOrEmpty(x.SubnetMask)) != this.Interfaces.Count)
                {
                    throw new IllegalStateException("The attachment is enabled for layer 3 but insufficient IPv4 addresses have been requested.");
                }
            }
            else if (this.Interfaces.Where(x => !string.IsNullOrEmpty(x.IpAddress) || !string.IsNullOrEmpty(x.SubnetMask)).Any())
            {
                throw new IllegalStateException("The attachment is NOT enabled for layer 3 but IPv4 addresses have been requested.");
            }

            if (this.AttachmentRole.RequireContractBandwidth)
            {
                if (this.ContractBandwidthPool?.ContractBandwidth == null)
                {
                    throw new IllegalStateException("A contract bandwidth for the attachment is required in accordance with the attachment role " +
                        $"of '{this.AttachmentRole.Name}' but none is defined.");
                }

                if (this.ContractBandwidthPool.ContractBandwidth.BandwidthMbps > this.AttachmentBandwidth.BandwidthGbps * 1000)
                {
                    throw new IllegalStateException($"The requested contract bandwidth of " +
                        $"{this.ContractBandwidthPool.ContractBandwidth.BandwidthMbps} Mbps is greater " +
                        $"than the bandwidth of the attachment which is {this.AttachmentBandwidth.BandwidthGbps} Gbps.");
                }
            }
            else
            {
                if (this.ContractBandwidthPool != null)
                {
                    throw new IllegalStateException("A contract bandwidth for the attachment is defined but is NOT required for the attachment role " +
                        $"of '{this.AttachmentRole.Name}'.");
                }
            }

            if (this.AttachmentRole.IsTaggedRole)
            {
                if (!this.IsTagged)
                {
                    throw new IllegalStateException("The attachment must be enabled for tagging with the 'isTagged' property in accordance with the " +
                        $"attachment role of '{this.AttachmentRole.Name}'.");
                }
                if (this.IsLayer3) throw new IllegalStateException("Layer 3 cannot be enabled concurrently with a tagged attachment.");
            }
            else
            {
                if (this.Vifs.Any())
                {
                    throw new IllegalStateException("Vifs were found for the attachment but the attachment is not enabled for tagging with the 'isTagged' properrty.");
                }
            }

            if (this.IsBundle)
            {
                if (!this.AttachmentRole.SupportedByBundle) throw new IllegalStateException($"The requested attachment role " +
                $"'{this.AttachmentRole.Name}' is not supported with a bundle attachment.");

                if (!this.AttachmentBandwidth.SupportedByBundle) throw new IllegalStateException($"The requested attachment " +
                $"bandwidth '{this.AttachmentBandwidth.BandwidthGbps} Gbps' is not supported with a bundle attachment.");

                var numPorts = this.Interfaces.SelectMany(x => x.Ports).Count();
                if (this.BundleMinLinks > numPorts) throw new IllegalStateException($"The min links parameter for the bundle " +
                    $"({this.BundleMinLinks}) must be " +
                    $"less than or equal to the total number of ports required for the bundle ({numPorts}).");

                if (this.BundleMaxLinks > numPorts) throw new IllegalStateException($"The max links parameter for the bundle " +
                    $"({this.BundleMaxLinks}) must be " +
                    $"less than or equal to the total number of ports required for the bundle ({numPorts}).");

                if (this.BundleMinLinks > this.BundleMaxLinks) throw new IllegalStateException($"The min links parameter for the bundle " +
                    $"({this.BundleMinLinks}) must be less then " +
                    $"or equal to the max links parameter for the bundle ({this.BundleMaxLinks})");
            }

            if (this.IsMultiPort)
            {
                if (!this.AttachmentRole.SupportedByMultiPort) throw new IllegalStateException($"The requested attachment role " +
                    $"'{this.AttachmentRole.Name}' is not supported with a multiport attachment.");

                if (!this.AttachmentBandwidth.SupportedByMultiPort) throw new IllegalStateException($"The requested attachment " +
                    $"bandwidth '{this.AttachmentBandwidth.BandwidthGbps} Gbps' is not supported with a multiport attachment.");
            }

            // Validate the routing instance if one exists
            if (this.RoutingInstance != null)
            {
                this.RoutingInstance.Validate();
            }
        }

        /// <summary>
        /// Validates that the ports of a an attachment are configured correctly.
        /// </summary>
        protected virtual void ValidatePortsConfiguredCorrectly()
        {
            var totalPortBandwidthGbps = (this.Interfaces
                                              .SelectMany(
                                                 x =>
                                                 x.Ports)
                                               .Where(
                                                 x =>
                                                 x.DeviceID == this.DeviceID)
                                               .Sum(
                                                 x =>
                                                 x.PortBandwidth.BandwidthGbps)
                                         );

            if (totalPortBandwidthGbps < this.AttachmentBandwidth.BandwidthGbps)
            {
                throw new IllegalStateException($"Attachment '{this.Name}' is misconfigured. The total port bandwidth "
                    + $"({totalPortBandwidthGbps} Gbps) is less than the required attachment bandwidth ({this.AttachmentBandwidth.BandwidthGbps} Gbps). "
                    + "Check that the correct number of ports are configured and that all of the ports are configured for the same device.");
            }
        }
    }
}