using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class ContractBandwidth : ValueObject
    {
        public int BandwidthMbps { get; private set; }
    }
}