using MINDOnContainers.BuildingBlocks.EventBus.Events;

namespace MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.Events
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
