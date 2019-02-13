using System.Threading.Tasks;
using MediatR;
using System;
using MINDOnContainers.BuildingBlocks.EventBus.Abstractions;
using MINDOnContainers.Services.Attachment.API.Application.Commands;
using MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.Events;

namespace MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.EventHandling
{
    public class UniCreatedIntegrationEventHandler :
            IIntegrationEventHandler<UniCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public UniCreatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(UniCreatedIntegrationEvent @event)
        {
            var command = new SetUniCommand(@event.AttachmentId, @event.UniName, @event.UniAccessLinkIdentifiers, @event.RoutingInstanceId);
            await _mediator.Send(command);
        }
    }
}