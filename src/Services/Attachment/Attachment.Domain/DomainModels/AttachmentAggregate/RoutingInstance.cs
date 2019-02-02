using System;
using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class RoutingInstance : Entity
    {
        public string Name { get; private set; }
        public int? AdministratorSubField { get; private set; }
        public int? AssignedNumberSubField { get; private set; }
        private readonly int? _tenantID;
        private readonly RouteDistinguisherRange _routeDistinguisherRange;
        public RoutingInstanceType RoutingInstanceType { get; private set; }
        private readonly Device _device;
        private readonly List<Attachment> _attachments;
        public IReadOnlyCollection<Attachment> Attachments => _attachments;
        private readonly List<Vif> _vifs { get; set; }
        public IReadOnlyCollection<Vif> Vifs => _vifs;
        private readonly List<BgpPeer> _bgpPeers;

        protected RoutingInstance()
        {
            _attachments = new List<Attachment>();
            _vifs = new List<Vif>();
            _bgpPeers = new List<BgpPeer>();
        }

        public RoutingInstance(string name, Device device, RouteDistinguisherRange range, RoutingInstanceType type, int? tenantId = null,
        int? administratorSubField = null, int? assignedNumberSubField = null) : this()
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (device.RoutingInstances.Any(routingInstance => routingInstance.Name == name))
                {
                    throw new AttachmentDomainException($"Routing instance name '{name}' is already used.");
                }

                _name = name;
            }
            else
            {
                _name = Guid.NewGuid().ToString("N");
            }

            Device = device ?? throw new ArgumentNullException(nameof(device));
            RoutingInstanceType = type ?? throw new ArgumentNullException(nameof(type));

            // Must assign a route distinguisher to this routing instance if the routing instance is for a VRF
            // Note the Default routing instance must not be assigned a route distinguisher
            if (type == RoutingInstanceType.ProviderDomainInfrastructureVrf || type == RoutingInstanceType.ProviderDomainTenantFacingLayer3Vrf)
            {
                AssignRouteDistinguisher();
            }
        }

        protected void AssignRouteDistinguisher(int? administratorSubField, int? assignedNumberSubField, RouteDistinguisherRange range)
        {
            if (administratorSubField.HasValue && assignedNumberSubField.HasValue)
            {
                if (this._device.RoutingInstances.Any(routingInstance =>
                            routingInstance.AssignedNumberSubField == assignedNumberSubField.Value &&
                            routingInstance.AdministratorSubField == administratorSubField.Value)
                {
                    throw new AttachmentDomainException($"A routing instance with route distinguisher '{administratorSubField}:{assignedNumberSubField}' already exists.");
                }

                AdministratorSubField = administratorSubField;
                AssignedNumberSubField = assignedNumberSubField;
            }
            else
            {
                if (range == null) throw new NullValueException("A route distinguisher range is required.");
                var usedAssignedNumbers = range.RoutingInstances
                                               .Select(routingInstance => 
                                                       routingInstance.AssignedNumberSubField);

                // Allocate a new unused RD from the RD range

                int? newAssignedNumberSubField = Enumerable.Range(range.AssignedNumberSubFieldStart, range.AssignedNumberSubFieldCount)
                               .Except(used).FirstOrDefault();

                AssignedNumberSubField = newAssignedNumberSubField ?? throw new AttachmentDomainException("Failed to allocate a free route distinguisher. "
                        + "Please contact your system administrator, or try another range.");

                AdministratorSubField = range.AdministratorSubField;
            }
        }

        /// <summary>
        /// Validates an attempt to delete the routing instance.
        /// </summary>
        public virtual void ValidateDelete()
        {
            var sb = new StringBuilder();

            if (this.RoutingInstanceType.IsDefault)
            {
                sb.Append($"Routing instance '{this.Name}' cannot be deleted because it is a default routing instance.");
            }

            if (this.Vifs.Any())
            {
                sb.Append($"Routing instance '{this.Name}' cannot be deleted because one or more VIFs belong to the routing instance." +
                    $"Delete the VIFs first, or remove them from the routing instance.");
            }

            if (this.Attachments.Any())
            {
                sb.Append($"Routing instance '{this.Name}' cannot be deleted because one or more attachments belong to the routing instance." +
                    $"Delete the attachments first, or remove them from the routing instance.");
            }

            (from attachmentSetRoutingInstance in this.AttachmentSetRoutingInstances
             from vpnAttachmentSet in attachmentSetRoutingInstance.AttachmentSet.VpnAttachmentSets
             select vpnAttachmentSet)
                 .ToList()
                    .ForEach(
                        vpnAttachmentSet =>
                        {
                            sb.Append($"Routing instance '{this.Name}' belongs to attachment set '{vpnAttachmentSet.AttachmentSet.Name}' " +
                            $"which is bound to VPN '{vpnAttachmentSet.Vpn.Name}' and therefore cannot be deleted. Remove attachment set '{vpnAttachmentSet.AttachmentSet.Name}' " +
                            $"from VPN '{vpnAttachmentSet.Vpn.Name}' first.");
                        });

            if (sb.Length > 0) throw new IllegalDeleteAttemptException(sb.ToString());
        }
    }
}