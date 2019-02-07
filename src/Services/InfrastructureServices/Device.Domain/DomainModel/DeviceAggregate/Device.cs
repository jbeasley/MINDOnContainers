using System;
using System.Collections.Generic;
using MINDOnContainers.Services.InfrastructureServices.Device.Domain.Exceptions;
using MINDOnContainers.Services.InfrastructureServices.Device.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Device.Domain.DomainModels.DeviceAggregate
{
    public class Device : Entity, IAggregateRoot
    {
        private readonly List<Port> _ports;
        public IReadOnlyCollection<Port> Ports => _ports;

        public Device()
        {
        }
    }
}
