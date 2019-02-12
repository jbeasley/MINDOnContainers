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
        public int NumPortsRequired { get; }
        public int PortBandwidthRequiredGbps { get; }
        public string LocationName { get; }
        public int PortPoolId { get; }
        public string PlaneName { get; }

        public AttachmentInitialisedDomainEvent(DomainModels.AttachmentAggregate.Attachment attachment, int numPortsRequired, 
        int portBandwidthRequiredGbps, string locationName, int portPoolId, string planeName = null)
        {
            Attachment = attachment;
            NumPortsRequired = numPortsRequired;
            PortBandwidthRequiredGbps = portBandwidthRequiredGbps;
            LocationName = locationName;
            PlaneName = planeName;
            PortPoolId = portPoolId;
        }
    }
}
