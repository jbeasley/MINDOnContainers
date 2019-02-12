using System.Threading.Tasks;
using MediatR;
using System;
using MINDOnContainers.BuildingBlocks.EventBus.Abstractions;
using MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.Commands;
using MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.Events;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.EventHandling
{
    public class AttachmentDeviceAndPortAssignmentsRequestedIntegrationEventHandler :
            IIntegrationEventHandler<AttachmentDeviceAndPortAssignmentsRequestedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public AttachmentDeviceAndPortAssignmentsRequestedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(AttachmentDeviceAndPortAssignmentsRequestedIntegrationEvent @event)
        {
            var command = new AssignDeviceAndAttachmentPortsCommand(@event.AttachmentId, @event.LocationName, @event.PortPoolId,
            @event.NumPortsRequired, @event.PortBandwidthRequiredGbps, @event.PlaneName, @event.TenantId);
            await _mediator.Send(command);
        }
    }
}