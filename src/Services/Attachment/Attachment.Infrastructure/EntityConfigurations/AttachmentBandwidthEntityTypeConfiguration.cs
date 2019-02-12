using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{ 
    class AttachmentBandwidthEntityTypeConfiguration : IEntityTypeConfiguration<Domain.DomainModels.AttachmentBandwidthAggregate.AttachmentBandwidth>
    {
        public void Configure(EntityTypeBuilder<Domain.DomainModels.AttachmentBandwidthAggregate.AttachmentBandwidth> attachmentBandwidthConfiguration)
        {
            attachmentBandwidthConfiguration.ToTable("attachmentBandwidth", AttachmentContext.DEFAULT_SCHEMA);
            attachmentBandwidthConfiguration.HasKey(o => o.Id);
            attachmentBandwidthConfiguration.Ignore(b => b.DomainEvents);

            attachmentBandwidthConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("attachmentbandwidthseq", AttachmentContext.DEFAULT_SCHEMA);
                

            attachmentBandwidthConfiguration.Property<int>("BandwidthGbps").IsRequired();
            attachmentBandwidthConfiguration.Property<bool>("MustBeBundleOrMultiPort").IsRequired();
            attachmentBandwidthConfiguration.Property<bool>("SupportedByBundle").IsRequired();
            attachmentBandwidthConfiguration.Property<bool>("SupportedByMultiPort").IsRequired();
            attachmentBandwidthConfiguration.Property<int>("BundleOrMultiPortMemberBandwidthGbps").IsRequired();
        }
    }
}