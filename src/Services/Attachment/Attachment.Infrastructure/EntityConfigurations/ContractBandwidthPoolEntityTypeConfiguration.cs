using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class ContractBandwidthPoolEntityTypeConfiguration : IEntityTypeConfiguration<ContractBandwidthPool>
    {
        public void Configure(EntityTypeBuilder<ContractBandwidthPool> contractBandwidthPoolConfiguration)
        {
            contractBandwidthPoolConfiguration.ToTable("contractBandwidthPool", AttachmentContext.DEFAULT_SCHEMA);
            contractBandwidthPoolConfiguration.HasKey(o => o.Id);
            contractBandwidthPoolConfiguration.Ignore(b => b.DomainEvents);

            contractBandwidthPoolConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("contractbandwidthpoolseq", AttachmentContext.DEFAULT_SCHEMA);

            //Value objects persisted as owned entity type supported since EF Core 2.0
            contractBandwidthPoolConfiguration.OwnsOne(o => o.ContractBandwidth);

            contractBandwidthPoolConfiguration.Property<string>("Name").IsRequired();
            contractBandwidthPoolConfiguration.Property<bool>("TrustReceivedCosAndDscp").IsRequired();
            contractBandwidthPoolConfiguration.Property<int>("TenantId").IsRequired(false);
                                    
        }
    }
}