using System;
using System.Collections.Generic;
using MINDOnContainers.BuildingBlocks.EventBus.Events;

namespace MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.Events
{
    public class UniCreatedIntegrationEvent : IntegrationEvent
    {
        public int AttachmentId { get; private set; }
        public string UniName { get; private set; }
        public int? RoutingInstanceId { get; private set; }
        public List<string> UniAccessLinkIdentifiers { get; private set; }

        public UniCreatedIntegrationEvent(int attachmentId, string uniName, List<string> uniAccessLinkIdentifiers, int? routingInstanceId)
        {
            AttachmentId = attachmentId;
            UniName = uniName;
            UniAccessLinkIdentifiers = uniAccessLinkIdentifiers;
            RoutingInstanceId = routingInstanceId;
        }
    }
}
