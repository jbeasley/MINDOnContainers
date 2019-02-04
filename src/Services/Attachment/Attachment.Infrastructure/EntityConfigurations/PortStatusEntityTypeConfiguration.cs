using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class PortStatusEntityTypeConfiguration : IEntityTypeConfiguration<PortStatus>
    {
        public void Configure(EntityTypeBuilder<PortStatus> portStatusConfiguration)
        {
            portStatusConfiguration.ToTable("portStatus", AttachmentContext.DEFAULT_SCHEMA);
            portStatusConfiguration.HasKey(o => o.Id);

            portStatusConfiguration.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            portStatusConfiguration.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();                
        }
    }
}