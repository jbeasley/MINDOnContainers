using System;
using System.Collections.Generic;
using MINDOnContainers.BuildingBlocks.EventBus.Events;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.Events
{
    public class UniCreationFailedIntegrationEvent : IntegrationEvent
    {
        public int AttachmentId { get; private set; }

        public UniCreationFailedIntegrationEvent(int attachmentId)
        {
            AttachmentId = attachmentId;
        }
    }
}
