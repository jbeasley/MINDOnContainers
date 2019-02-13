using MediatR;

namespace MINDOnContainers.Services.Attachment.Domain.Events
{
    /// <summary>
    /// Event raised when a new attachment has been created.
    /// </summary>
    public class AttachmentCreatedDomainEvent : INotification
    {
        public DomainModels.AttachmentAggregate.Attachment Attachment { get; }
        public int NumPortsRequired { get; }
        public int PortBandwidthRequiredGbps { get; }
        public string LocationName { get; }
        public int PortPoolId { get; }
        public string PlaneName { get; }
        public bool RequireRoutingInstance { get; }

        public AttachmentCreatedDomainEvent(DomainModels.AttachmentAggregate.Attachment attachment, int numPortsRequired, 
        int portBandwidthRequiredGbps, string locationName, int portPoolId, bool requireRoutingInstance, string planeName = null)
        {
            Attachment = attachment;
            NumPortsRequired = numPortsRequired;
            PortBandwidthRequiredGbps = portBandwidthRequiredGbps;
            LocationName = locationName;
            PlaneName = planeName;
            RequireRoutingInstance = requireRoutingInstance;
            PortPoolId = portPoolId;
        }
    }
}
