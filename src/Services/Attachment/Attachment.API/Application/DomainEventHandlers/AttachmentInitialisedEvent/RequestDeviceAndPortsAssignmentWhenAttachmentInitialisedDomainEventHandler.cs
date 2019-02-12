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
    public class RequestDeviceAndPortsAssignmentWhenAttachmentInitialisedDomainEventHandler
                    : INotificationHandler<AttachmentInitialisedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IAttachmentRepository _attachmentRepository;       
        private readonly IAttachmentIntegrationEventService _attachmentIntegrationEventService;

        public RequestDeviceAndPortsAssignmentWhenAttachmentInitialisedDomainEventHandler(
            ILoggerFactory logger,
            IAttachmentRepository attachmentRepository,
            IAttachmentIntegrationEventService attachmentIntegrationEventService)
        {
            _attachmentRepository = attachmentRepository ?? throw new ArgumentNullException(nameof(attachmentRepository));                               
            _attachmentIntegrationEventService = attachmentIntegrationEventService ?? throw new ArgumentNullException(nameof(attachmentIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AttachmentInitialisedDomainEvent @event, CancellationToken cancellationToken)
        {           
            await _attachmentRepository.UnitOfWork.SaveEntitiesAsync();

            var attachmentAwaitingDeviceAndPortAssignmentsIntegrationEvent =
                new AttachmentDeviceAndPortAssignmentsRequestedIntegrationEvent(@event.Attachment.Id, @event.LocationName, @event.NumPortsRequired,
                @event.PortBandwidthRequiredGbps, @event.PortPoolId, @event.PlaneName);

            await _attachmentIntegrationEventService.AddAndSaveEventAsync(attachmentAwaitingDeviceAndPortAssignmentsIntegrationEvent);

            _logger.CreateLogger(nameof(RequestDeviceAndPortsAssignmentWhenAttachmentInitialisedDomainEventHandler))
                    .LogTrace($"Attachment {@event.Attachment.Name} was initialised and awaiting port assignments.");
        }
    }
}