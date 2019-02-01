using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Device : Entity
    {
        public string Name { get; private set};
        private readonly List<Port> _ports;
        public IReadOnlyCollection<Port> Ports => _ports;
        private readonly List<RoutingInstance> _routingInstances;
        public IReadOnlyCollection<RoutingInstance> RoutingInstances => _routingInstances;
        public DeviceRole DeviceRole { get; private set; }

        public Device(string name, DeviceRole deviceRole, List<Port> ports, List<RoutingInstance> routingInstances)
        {
            _name = name;
            DeviceRole = deviceRole;
            _ports = ports;
            _routingInstances = routingInstances;
        }
    }
}