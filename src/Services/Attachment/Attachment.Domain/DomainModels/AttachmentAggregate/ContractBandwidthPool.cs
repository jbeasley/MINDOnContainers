using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{

    public class ContractBandwidthPool : Entity
    {
        private string _name;
        private bool _trustReceivedCosAndDscp;
        private int? _tenantID { get; set; }
        private ContractBandwidth _contractBandwidth;

        /// <summary>
        /// Validates deletion of the contract bandwidth pool.
        /// </summary>
        public virtual void ValidateDelete()
        {
            var sb = new StringBuilder();

            if (this.Vifs.Any())
            {
                sb.Append($"Contract Bandwidth Pool '{this.Name}' cannot be deleted because one or more VIFs belong to the contract bandwidth pool." +
                    $"Delete the VIFs first, or remove them from the contract bandwidth pool.");
            }

            if (this.Attachments.Any())
            {
                sb.Append($"Contract Bandwidth Pool '{this.Name}' cannot be deleted because one or more attachments belong to the contract bandwidth pool." +
                    $"Delete the attachments first, or remove them from the contract bandwidth pool.");
            }
        }
    }
}
