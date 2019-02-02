using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Device : Entity
    {
        public string Name { get; private set; } 
        private readonly List<Port> _ports;
        public IReadOnlyCollection<Port> Ports => _ports;
        private readonly List<RoutingInstance> _routingInstances;
        public IReadOnlyCollection<RoutingInstance> RoutingInstances => _routingInstances;
        public DeviceRole DeviceRole { get; private set; }

        protected Device()
        {
            _ports = new List<Port>();
            _routingInstances = new List<RoutingInstance>();
        }

        public Device(string name, DeviceRole deviceRole, List<Port> ports, List<RoutingInstance> routingInstances) : this()
        {
            !string.IsNullOrEmpty(name) ? _name = name : throw new ArgumentNullException(nameof(name));
            DeviceRole = deviceRole ?? throw new ArgumentNullException(nameof(deviceRole));

            ports.ForEach(port => AddPort(port));
            routingInstances.ForEach(routingInstance => AddRoutingInstance(routingInstance));
        }

        public AddRoutingInstance(RoutingInstance routingInstance)
        {
            if (deviceRole.IsProviderDomainRole)
            {
                if (routingInstance.RoutingInstanceType == RoutingInstanceType.ProviderDomainTenantFacingLayer3Vrf ||
                    RoutingInstanceType == RoutingInstanceType.ProviderDomainInfrastructureVrf)
                {
                    _routingInstances.Add(routingInstance);
                }
            }
        }

        public AddPort(Port port)
        {
            // Get the collection of acceptable port pools for the device role of this device
            // The port can be added only if it belongs to an acceptable port pool
            var acceptablePortPoolIds = deviceRole.GetPortPoolIds();

            if (acceptablePortPoolIds.Contains(port.GetPortPoolId())
            {
                _ports.Add(port);
            }
        }
    }
}