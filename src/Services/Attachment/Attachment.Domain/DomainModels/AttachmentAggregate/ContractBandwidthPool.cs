using System;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class ContractBandwidthPool : Entity
    {
        private readonly string _name;
        private readonly bool _trustReceivedCosAndDscp;
        private readonly int? _tenantId;
        public ContractBandwidth ContractBandwidth { get; private set; }

        public ContractBandwidthPool(ContractBandwidth contractBandwidth, bool trustReceivedCosAndDscp = false, int? tenantId = null)
        {
            _name = Guid.NewGuid().ToString("N");
            _trustReceivedCosAndDscp = trustReceivedCosAndDscp;
            ContractBandwidth = contractBandwidth;
            _tenantId = tenantId;
        }
    }
}
