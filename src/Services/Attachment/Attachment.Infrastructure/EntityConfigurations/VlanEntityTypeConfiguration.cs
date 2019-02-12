using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class VlanEntityTypeConfiguration : IEntityTypeConfiguration<Vlan>
    {
        public void Configure(EntityTypeBuilder<Vlan> vlanConfiguration)
        {
            vlanConfiguration.ToTable("vlan", AttachmentContext.DEFAULT_SCHEMA);
            vlanConfiguration.HasKey(o => o.Id);
            vlanConfiguration.Ignore(b => b.DomainEvents);

            vlanConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("vlanseq", AttachmentContext.DEFAULT_SCHEMA);

            vlanConfiguration.Property<int>("Ipv4AddressAndMaskId").IsRequired(false);

            vlanConfiguration.HasOne<Ipv4AddressAndMask>()
                .WithOne()
                .HasForeignKey("Ipv4AddressAndMaskId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);                
        }
    }
}