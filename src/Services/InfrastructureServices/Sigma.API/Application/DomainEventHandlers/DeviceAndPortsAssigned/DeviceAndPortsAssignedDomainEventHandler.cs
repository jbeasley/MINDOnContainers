using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Events;
using MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents;
using MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.Events;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.DomainEventHandlers
{
    public class DeviceAndPortsAssignedDomainEventHandler
                    : INotificationHandler<DeviceAndPortsAssignedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly ISigmaRepository _sigmaRepository;       
        private readonly ISigmaIntegrationEventService _sigmaIntegrationEventService;

        public DeviceAndPortsAssignedDomainEventHandler(
            ILoggerFactory logger,
            ISigmaRepository sigmaRepository,
            ISigmaIntegrationEventService sigmaIntegrationEventService)
        {
            _sigmaRepository = sigmaRepository ?? throw new ArgumentNullException(nameof(sigmaRepository));                               
            _sigmaIntegrationEventService = sigmaIntegrationEventService ?? throw new ArgumentNullException(nameof(sigmaIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(DeviceAndPortsAssignedDomainEvent @event, CancellationToken cancellationToken)
        {
            var deviceAndPortsAssignedIntegrationEvent =
                new DeviceAndPortsAssignedIntegrationEvent(@event.AttachmentId, @event.DeviceId, @event.AssignedPorts);

            await _sigmaIntegrationEventService.AddAndSaveEventAsync(deviceAndPortsAssignedIntegrationEvent);

            _logger.CreateLogger(nameof(DeviceAndPortsAssignedDomainEventHandler))
                    .LogTrace($"Attachment with ID {@event.AttachmentId} has been assigned to device with ID {@event.DeviceId} and has been assigned ports:");
                
        }
    }
}