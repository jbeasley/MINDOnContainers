using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class RoutingInstance : Entity
    {
        public string Name { get; private set; }
        public int? AdministratorSubField { get; private set; }
        public int? AssignedNumberSubField { get; private set; }
        private readonly int? _tenantId;
        private readonly RouteDistinguisherRange _routeDistinguisherRange;
        public RoutingInstanceType RoutingInstanceType { get; private set; }
        private readonly Device _device;
        private readonly List<Attachment> _attachments;
        public IReadOnlyCollection<Attachment> Attachments => _attachments;
        private readonly List<Vif> _vifs;
        public IReadOnlyCollection<Vif> Vifs => _vifs;
        private readonly List<BgpPeer> _bgpPeers;

        protected RoutingInstance()
        {
            _attachments = new List<Attachment>();
            _vifs = new List<Vif>();
            _bgpPeers = new List<BgpPeer>();
        }

        public RoutingInstance(Device device, RoutingInstanceType type, string name = "", int? tenantId = null, 
        RouteDistinguisherRange range = null, int? administratorSubField = null, int? assignedNumberSubField = null) : this()
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (device.RoutingInstances.Any(routingInstance => routingInstance.Name == name))
                {
                    throw new AttachmentDomainException($"Routing instance name '{name}' is already used.");
                }

                Name = name;
            }
            else
            {
                Name = Guid.NewGuid().ToString("N");
            }

            _device = device ?? throw new ArgumentNullException(nameof(device));
            RoutingInstanceType = type ?? throw new ArgumentNullException(nameof(type));
            _routeDistinguisherRange = range;

            // Must assign a route distinguisher to this routing instance if the routing instance is for a VRF
            // Note the Default routing instance must not be assigned a route distinguisher
            if (type == RoutingInstanceType.Vrf)
            {
                AssignRouteDistinguisher(administratorSubField, assignedNumberSubField, range);
            }

            this._tenantId = tenantId;
        }

        protected void AssignRouteDistinguisher(int? administratorSubField, int? assignedNumberSubField, RouteDistinguisherRange range)
        {
            if (administratorSubField.HasValue && assignedNumberSubField.HasValue)
            {
                if (this._device.RoutingInstances.Any(routingInstance =>
                            routingInstance.AssignedNumberSubField == assignedNumberSubField.Value &&
                            routingInstance.AdministratorSubField == administratorSubField.Value))
                {
                    throw new AttachmentDomainException($"A routing instance with route distinguisher '{administratorSubField}:{assignedNumberSubField}' already exists.");
                }

                AdministratorSubField = administratorSubField;
                AssignedNumberSubField = assignedNumberSubField;
            }
            else
            {
                if (range == null) throw new ArgumentNullException(nameof(range));

                var usedAssignedNumbers = range.RoutingInstances
                                               .Where(routingInstance => 
                                                      routingInstance.AssignedNumberSubField.HasValue)
                                               .Select(routingInstance =>
                                                       routingInstance.AssignedNumberSubField.Value)
                                               .ToList();

                // Allocate a new unused RD from the RD range

                int? newAssignedNumberSubField = Enumerable.Range(range.AssignedNumberSubFieldStartValue, range.GetCount())
                               .Except(usedAssignedNumbers).FirstOrDefault();

                AssignedNumberSubField = newAssignedNumberSubField ?? throw new AttachmentDomainException("Failed to allocate a free route distinguisher. "
                        + "Please contact your system administrator, or try another range.");

                AdministratorSubField = range.AdministratorSubField;
            }
        }
    }
}