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
    public class UniCreatedDomainEventHandler
                    : INotificationHandler<UniCreatedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IMapper _mapper;       
        private readonly ISigmaIntegrationEventService _sigmaIntegrationEventService;

        public UniCreatedDomainEventHandler(
            ILoggerFactory logger,
            IMapper mapper,
            ISigmaIntegrationEventService sigmaIntegrationEventService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));                               
            _sigmaIntegrationEventService = sigmaIntegrationEventService ?? throw new ArgumentNullException(nameof(sigmaIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(UniCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            var uniDTO = _mapper.Map<UniDTO>(@event.Uni);
            var uniCreatedIntegrationEvent = new UniCreatedIntegrationEvent(@event.AttachmentId, uniDTO);

            await _sigmaIntegrationEventService.AddAndSaveEventAsync(uniCreatedIntegrationEvent);

            _logger.CreateLogger(nameof(UniCreatedDomainEventHandler))
                    .LogTrace($"Attachment with ID {@event.AttachmentId} has been assigned UNI {@event.Uni.Name}.");
                
        }
    }
}