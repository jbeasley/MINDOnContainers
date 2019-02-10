using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Exceptions;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    public class Device : Entity
    {
        public string Name { get; private set; }
        public Location Location { get; private set; }
        public Plane Plane { get; private set; }
        public DeviceStatus Status { get; private set; }
        private readonly List<Port> _ports;
        public IReadOnlyCollection<Port> Ports => _ports;
        private readonly List<RoutingInstance> _routingInstances;
        public IReadOnlyCollection<RoutingInstance> RoutingInstances => _routingInstances;

        protected Device()
        {
            _ports = new List<Port>();
            _routingInstances = new List<RoutingInstance>();

            var defaultRoutingInstance = new RoutingInstance(this, RoutingInstanceType.Default, "Default");
            this.AddRoutingInstance(defaultRoutingInstance);
        }

        public Device(string name, Plane plane, Location location) : this()
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.Plane = plane ?? throw new ArgumentNullException(nameof(plane));
            this.Location = location ?? throw new ArgumentNullException(nameof(location));
        }

        public void AddRoutingInstance(RoutingInstance routingInstance)
        {
            if (this.RoutingInstances.Contains(routingInstance))
            {
                throw new SigmaDomainException($"Routing instance '{routingInstance.Name}' already exists for device '{this.Name}'.");
            }
            this._routingInstances.Add(routingInstance);
        }

        public int GetDeviceId() => this.Id;
    }
}
