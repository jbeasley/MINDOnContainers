using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate

{
    public class RoutingInstance : Entity
    {
        private string _name { get; set; }
        private int? _administratorSubField { get; set; }
        private int? _assignedNumberSubField { get; set; }
        private int _deviceID { get; set; }
        private int? _tenantID { get; set; }
        private int? _routeDistinguisherRangeID { get; set; }
        private int _routingInstanceTypeID { get; set; }
        private Device _device { get; set; }
        private int _routingInstanceTypeId { get; set; }
        private List<Attachment> _attachments { get; set; }
        public IReadOnlyCollection<Attachment> Attachments => _attachments;
        private List<Vif> _vifs { get; set; }
        private List<BgpPeer> _bgpPeers { get; set; }

        /// <summary>
        /// Validate the state of the routing instance
        /// </summary>
        public virtual void Validate()
        {
            if (this.Device == null) throw new IllegalStateException("A device must be defined for the routing instance.");
            if (this.RoutingInstanceType == null) throw new IllegalStateException("A routing instance type must be defined for the " +
                    "routing instance.");
            if (this.RoutingInstanceType.IsVrf)
            {
                if (!this.AdministratorSubField.HasValue) throw new IllegalStateException("An administrator subfield value must be defined " +
                    "for the routing instance.");
                if (!this.AssignedNumberSubField.HasValue) throw new IllegalStateException("An assigned number subfield value must be defined " +
                    "for the routing instance.");
                if (this.RouteDistinguisherRange == null) throw new IllegalStateException("A route distinguisher range must be defined " +
                    "for the routing instance.");
                if (!this.RoutingInstanceType.IsTenantFacingVrf && !this.RoutingInstanceType.IsInfrastructureVrf)
                    throw new IllegalStateException("The routing instance must be defined as either a tenant-facing vrf routing instance or an " +
                        "infrastructure vrf routing instance");
                if (this.RoutingInstanceType.IsTenantFacingVrf)
                {
                    if (this.Tenant == null)
                    {
                        throw new IllegalStateException("A tenant must be defined because the routing instance is defined as a tenant-facing vrf " +
                            "routing instance.");
                    }
                }

                if (this.Device.RoutingInstances.Any(x =>
                            x.Name == this.Name &&
                            x.RoutingInstanceID != this.RoutingInstanceID))
                {
                    throw new IllegalStateException($"The name '{this.Name}' for the routing instance is already used.");
                }

                if (this.Device.RoutingInstances.Any(x =>
                        x.AdministratorSubField == this.AdministratorSubField &&
                        x.AssignedNumberSubField == this.AssignedNumberSubField &&
                        x.RoutingInstanceID != this.RoutingInstanceID))
                {
                    throw new IllegalStateException($"The administrator subfield '{this.AdministratorSubField}' and " +
                        $"assigned number subfield '{this.AssignedNumberSubField}' values for the routing instance with name '{this.Name}' are already used.");
                }
            }
            else if (this.RoutingInstanceType.IsDefault)
            {
                if (this.AdministratorSubField.HasValue) throw new IllegalStateException("An administrator subfield value must not be defined " +
                    "for the default routing instance.");
                if (this.AssignedNumberSubField.HasValue) throw new IllegalStateException("An assigned number subfield value must not be defined " +
                    "for the default routing instance.");
                if (this.RouteDistinguisherRange != null) throw new IllegalStateException("A route distinguisher range must not be defined " +
                    "for the default routing instance.");
                if (this.Tenant != null)
                {
                    throw new IllegalStateException("A tenant must not be defined because the routing instance is defined as a default " +
                        "routing instance.");
                }
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