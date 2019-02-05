using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class Ipv4AddressAndMaskEntityTypeConfiguration : IEntityTypeConfiguration<Ipv4AddressAndMask>
    {
        public void Configure(EntityTypeBuilder<Ipv4AddressAndMask> ipv4AddressAndMaskConfiguration)
        {
            ipv4AddressAndMaskConfiguration.ToTable("ipv4AddressAndMask", AttachmentContext.DEFAULT_SCHEMA);
            ipv4AddressAndMaskConfiguration.HasKey(o => o.Id);
            ipv4AddressAndMaskConfiguration.Ignore(b => b.DomainEvents);

            ipv4AddressAndMaskConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("ipv4addressandmaskseq", AttachmentContext.DEFAULT_SCHEMA);
                
            ipv4AddressAndMaskConfiguration.Property<string>("Ipv4Address").IsRequired();
            ipv4AddressAndMaskConfiguration.Property<string>("Ipv4SubnetMask").IsRequired();
                            
        }
    }
}