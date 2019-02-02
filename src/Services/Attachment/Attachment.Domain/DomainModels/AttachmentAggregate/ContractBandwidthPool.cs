using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class ContractBandwidthPool : Entity
    {
        private readonly string _name;
        private readonly bool _trustReceivedCosAndDscp;
        private readonly int? _tenantId;
        private readonly ContractBandwidth _contractBandwidth;


    }
}
