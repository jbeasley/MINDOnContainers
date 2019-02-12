using System.Threading.Tasks;
using MediatR;
using System;
using MINDOnContainers.BuildingBlocks.EventBus.Abstractions;
using MINDOnContainers.Services.Attachment.API.Application.Commands;
using MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.Events;

namespace MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.EventHandling
{
    public class AttachmentDeviceAndPortsAssignedIntegrationEventHandler :
            IIntegrationEventHandler<AttachmentDeviceAndPortsAssignedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public AttachmentDeviceAndPortsAssignedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(AttachmentDeviceAndPortsAssignedIntegrationEvent @event)
        {
            var command = new SetDeviceAndAttachmentPortsCommand(@event.AttachmentId, @event.DeviceId, @event.DeviceName, @event.PortAssignments);
            await _mediator.Send(command);
        }
    }
}