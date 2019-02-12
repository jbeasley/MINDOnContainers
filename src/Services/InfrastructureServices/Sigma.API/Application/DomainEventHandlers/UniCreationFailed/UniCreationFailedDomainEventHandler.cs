using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Events;
using MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents;
using MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.Events;
using static MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.Events.UniCreatedIntegrationEvent;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.DomainEventHandlers
{
    public class UniCreationFailedDomainEventHandler
                    : INotificationHandler<UniCreationFailedDomainEvent>
    {
        private readonly ILoggerFactory _logger;     
        private readonly ISigmaIntegrationEventService _sigmaIntegrationEventService;

        public UniCreationFailedDomainEventHandler(
            ILoggerFactory logger,
            IMapper mapper,
            ISigmaIntegrationEventService sigmaIntegrationEventService)
        {                             
            _sigmaIntegrationEventService = sigmaIntegrationEventService ?? throw new ArgumentNullException(nameof(sigmaIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(UniCreationFailedDomainEvent @event, CancellationToken cancellationToken)
        {
            var uniCreationFailedIntegrationEvent = new UniCreationFailedIntegrationEvent(@event.AttachmentId);

            await _sigmaIntegrationEventService.AddAndSaveEventAsync(uniCreationFailedIntegrationEvent);

            _logger.CreateLogger(nameof(UniCreationFailedDomainEventHandler))
                    .LogTrace($"Failed to create a UNI for attachment with ID {@event.AttachmentId}.");
                
        }
    }
}