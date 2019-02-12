using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class ContractBandwidth : ValueObject
    {
        public int BandwidthMbps { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return BandwidthMbps;
        }
    }
}