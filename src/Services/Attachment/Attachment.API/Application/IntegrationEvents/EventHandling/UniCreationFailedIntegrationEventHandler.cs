using System.Threading.Tasks;
using MediatR;
using System;
using MINDOnContainers.BuildingBlocks.EventBus.Abstractions;
using MINDOnContainers.Services.Attachment.API.Application.Commands;
using MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.Events;

namespace MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.EventHandling
{
    public class UniCreationFailedIntegrationEventHandler :
            IIntegrationEventHandler<UniCreationFailedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public UniCreationFailedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(UniCreationFailedIntegrationEvent @event)
        {
            var command = new SetAttachmentStatusCreationFailedCommand(@event.AttachmentId);
            await _mediator.Send(command);
        }
    }
}