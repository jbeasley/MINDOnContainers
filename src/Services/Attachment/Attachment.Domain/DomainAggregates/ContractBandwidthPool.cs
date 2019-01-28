using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.NovaAttSwagger.Model;

namespace SCM.Models
{
    /// <summary>
    /// ContractBandwidthPool nova client dto extensions.
    /// </summary>
    public static class ContractBandwidthPoolNovaClientDtoExtensions
    {
        /// <summary>
        /// Create an instance of the nova contract bandwidth pool dto.
        /// </summary>
        /// <returns>The nova contract bandwidth pool dto.</returns>
        /// <param name="contractBandwidthPool">An instance of ContractBandwidthPool</param>
        public static DataAttachmentAttachmentPePePeNameTaggedAttachmentInterfaceTaggedAttachmentInterfaceInterfaceTypeTaggedAttachmentInterfaceInterfaceIdContractBandwidthPoolContractBandwidthPoolName ToNovaContractBandwidthPoolDto(this ContractBandwidthPool contractBandwidthPool)
        {
            var data = new DataAttachmentAttachmentPePePeNameTaggedAttachmentInterfaceTaggedAttachmentInterfaceInterfaceTypeTaggedAttachmentInterfaceInterfaceIdContractBandwidthPoolContractBandwidthPoolName
            {
                AttachmentcontractBandwidthPool = new List<DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidContractbandwidthpoolContractbandwidthpoolnameAttachmentcontractbandwidthpool>
                {
                    new DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidContractbandwidthpoolContractbandwidthpoolnameAttachmentcontractbandwidthpool
                    {
                        Name = contractBandwidthPool.Name,
                        ContractBandwidth = Enum.Parse<DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidContractbandwidthpoolContractbandwidthpoolnameAttachmentcontractbandwidthpool
                                              .ContractBandwidthEnum>(contractBandwidthPool.ContractBandwidth.BandwidthMbps.ToString()),
                        TrustReceivedCosAndDscp = contractBandwidthPool.TrustReceivedCosAndDscp.ToString().ToLower(),
                        ServiceClasses = new List<DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameAttachmentserviceclasses>
                        {
                            new DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameAttachmentserviceclasses
                            {
                                ScName = DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameAttachmentserviceclasses.ScNameEnum.SIGMA20MarketDataTCP,
                                ScBandwidth =  new DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameScbandwidthAttachmentscbandwidth
                                {
                                    BwUnits = DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameScbandwidthAttachmentscbandwidth.BwUnitsEnum.Mbps,
                                    Bandwidth = contractBandwidthPool.ContractBandwidth.BandwidthMbps,
                                    BurstSize = CalculateBurstSizeBytes(contractBandwidthPool.ContractBandwidth.BandwidthMbps)
                                }
                            }
                        }
                    }
                }
            };

            return data;
        }

        /// <summary>
        /// Calculates the burst size bytes.
        /// </summary>
        /// <returns>The burst size bytes.</returns>
        /// <param name="contractBandwidthMbps">Contract bandwidth mbps.</param>
        private static int CalculateBurstSizeBytes(double contractBandwidthMbps)
        {
            double bc = 0.05;
            var burstBytes = (contractBandwidthMbps * 1000000 * bc) / 8;
            return Convert.ToInt32(burstBytes);
        }
    }

    public class ContractBandwidthPool
    {
        public int ContractBandwidthPoolID { get; private set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public int ContractBandwidthID { get; set; }
        public bool TrustReceivedCosAndDscp { get; set; }
        public int? TenantID { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual ContractBandwidth ContractBandwidth { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Vif> Vifs { get; set; }

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
