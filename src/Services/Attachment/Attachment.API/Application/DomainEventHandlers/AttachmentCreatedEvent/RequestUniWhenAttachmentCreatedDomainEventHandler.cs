using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;
using MINDOnContainers.Services.Attachment.Domain.Events;
using MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents;
using MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.Events;

namespace MINDOnContainers.Services.Attachment.API.Application.DomainEventHandlers
{
    public class RequestUniWhenAttachmentCreatedDomainEventHandler
                    : INotificationHandler<AttachmentCreatedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IAttachmentRepository _attachmentRepository;       
        private readonly IAttachmentIntegrationEventService _attachmentIntegrationEventService;

        public RequestUniWhenAttachmentCreatedDomainEventHandler(
            ILoggerFactory logger,
            IAttachmentRepository attachmentRepository,
            IAttachmentIntegrationEventService attachmentIntegrationEventService)
        {
            _attachmentRepository = attachmentRepository ?? throw new ArgumentNullException(nameof(attachmentRepository));                               
            _attachmentIntegrationEventService = attachmentIntegrationEventService ?? throw new ArgumentNullException(nameof(attachmentIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentCreatedDomainEvent @event, CancellationToken cancellationToken)
        {           
            var requestUniIntegrationEvent =
                new RequestUniWhenAttachmentCreatedIntegrationEvent(@event.Attachment.Id, @event.LocationName, @event.NumPortsRequired,
                @event.PortBandwidthRequiredGbps, @event.PortPoolId, @event.RequireRoutingInstance, @event.PlaneName);

            await _attachmentIntegrationEventService.AddAndSaveEventAsync(requestUniIntegrationEvent);

            _logger.CreateLogger(nameof(RequestUniWhenAttachmentCreatedDomainEventHandler))
                    .LogTrace($"Attachment {@event.Attachment.Name} was created and awaiting a UNI assignment.");
        }
    }
}