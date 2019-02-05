using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class BundleAttachmentEntityTypeConfiguration : IEntityTypeConfiguration<BundleAttachment>
    {
        public void Configure(EntityTypeBuilder<BundleAttachment> attachmentConfiguration)
        {
            attachmentConfiguration.Property<string>("BundleMinLinks").IsRequired();
            attachmentConfiguration.Property<string>("BundleMaxLinks").IsRequired();
        }
    }
}