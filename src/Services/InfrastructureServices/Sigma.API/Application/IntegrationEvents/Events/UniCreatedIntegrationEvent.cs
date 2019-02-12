using System;
using System.Collections.Generic;
using MINDOnContainers.BuildingBlocks.EventBus.Events;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.Events
{
    public class UniCreatedIntegrationEvent : IntegrationEvent
    {
        public int AttachmentId { get; private set; }
        public UniDTO Uni { get; private set; }

        public UniCreatedIntegrationEvent(int attachmentId, UniDTO uni)
        {
            AttachmentId = attachmentId;
            Uni = uni;
        }

        public class UniDTO
        {
            public int UniId { get; set; }
            public string UniName { get; set; }
            public List<string> UniAccessLinkIdentifiers { get; set; }
            public int? RoutingInstanceId { get; set; }
        }
    }
}
