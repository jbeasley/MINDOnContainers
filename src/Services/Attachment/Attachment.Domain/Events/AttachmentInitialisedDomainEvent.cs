using MediatR;

namespace MINDOnContainers.Services.Attachment.Domain.Events
{
    /// <summary>
    /// Event raised when a new single attachment has been created and is ready to be 
    /// persisted to inventory.
    /// </summary>
    public class AttachmentInitialisedDomainEvent : INotification
    {
        public DomainModels.AttachmentAggregate.Attachment Attachment { get; }

        public AttachmentInitialisedDomainEvent(DomainModels.AttachmentAggregate.Attachment attachment)
        {
            Attachment = attachment;
        }
    }
}
