using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Exceptions;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    public class RoutingInstance : Entity
    {
        public string Name { get; private set; }
        public int? AdministratorSubField { get; private set; }
        public int? AssignedNumberSubField { get; private set; }
        private readonly int? _tenantId;
        private readonly RouteDistinguisherRange _routeDistinguisherRange;
        private readonly int _routingInstanceTypeId;
        public RoutingInstanceType RoutingInstanceType { get; private set; }
        private readonly int _deviceId;
        private readonly Device _device;

        protected RoutingInstance()
        {
        }

        public RoutingInstance(Device device, RoutingInstanceType type, string name = "", int? tenantId = null, 
        RouteDistinguisherRange range = null, int? administratorSubField = null, int? assignedNumberSubField = null) : this()
        {
            this._device = device ?? throw new ArgumentNullException(nameof(device));
            this._deviceId = device.Id;

            if (!string.IsNullOrEmpty(name))
            {
                if (device.RoutingInstances.Any(routingInstance => routingInstance.Name == name))
                {
                    throw new SigmaDomainException($"Routing instance name '{name}' is already used. Please choose another.");
                }

                this.Name = name;
            }
            else
            {
                this.Name = Guid.NewGuid().ToString("N");
            }

            this.RoutingInstanceType = type ?? throw new ArgumentNullException(nameof(type));
            this._routingInstanceTypeId = type.Id;
            this._routeDistinguisherRange = range;

            // Must assign a route distinguisher to this routing instance if the routing instance type is VRF
            // Note the Default routing instance type must not be assigned a route distinguisher
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
                    throw new SigmaDomainException($"A routing instance with route distinguisher " +
                    	"'{administratorSubField}:{assignedNumberSubField}' already exists.");
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

                AssignedNumberSubField = newAssignedNumberSubField ?? throw new SigmaDomainException("Failed to allocate a free route distinguisher. " +
                        "Please contact your system administrator, or try another range.");

                AdministratorSubField = range.AdministratorSubField;
            }
        }
    }
}