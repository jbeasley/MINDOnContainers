using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class AttachmentStatusEntityTypeConfiguration : IEntityTypeConfiguration<AttachmentStatus>
    {
        public void Configure(EntityTypeBuilder<AttachmentStatus> attachmentStatusConfiguration)
        {
            attachmentStatusConfiguration.ToTable("attachmentStatus", AttachmentContext.DEFAULT_SCHEMA);
            attachmentStatusConfiguration.HasKey(o => o.Id);

            attachmentStatusConfiguration.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            attachmentStatusConfiguration.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();                
        }
    }
}