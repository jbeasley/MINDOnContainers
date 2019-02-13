using System;
using System.Collections.Generic;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    public class Uni : Entity
    {
        public string Name { get; private set; }
        private readonly int? _tenantId;
        private readonly Device _device;
        private readonly List<Port> _ports;
        public IReadOnlyCollection<Port> Ports => _ports;
        public RoutingInstance RoutingInstance { get; private set; }

        public Uni(Device device, List<Port> ports, RoutingInstance routingInstance = null, int? tenantId = null)
        {
            this.Name = Guid.NewGuid().ToString("N");
            this._device = device ?? throw new ArgumentNullException(nameof(device));
            this._ports = ports ?? throw new ArgumentNullException(nameof(ports));
            this.RoutingInstance = routingInstance;
            this._tenantId = tenantId;
        }
    }
}
