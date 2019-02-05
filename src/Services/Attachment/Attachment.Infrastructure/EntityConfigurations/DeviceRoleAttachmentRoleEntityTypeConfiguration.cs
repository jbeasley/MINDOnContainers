using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class DeviceRoleAttachmentRoleEntityTypeConfiguration : IEntityTypeConfiguration<DeviceRoleAttachmentRole>
    {
        public void Configure(EntityTypeBuilder<DeviceRoleAttachmentRole> deviceRoleAttachmentRoleConfiguration)
        {
            deviceRoleAttachmentRoleConfiguration.ToTable("deviceRoleAttachmentRole", AttachmentContext.DEFAULT_SCHEMA);
            deviceRoleAttachmentRoleConfiguration.HasKey(o => o.Id);
            deviceRoleAttachmentRoleConfiguration.Ignore(b => b.DomainEvents);

            deviceRoleAttachmentRoleConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("deviceroleattachmentroleseq", AttachmentContext.DEFAULT_SCHEMA);

            deviceRoleAttachmentRoleConfiguration.Property<int>("DeviceRoleId").IsRequired();
            deviceRoleAttachmentRoleConfiguration.Property<int>("AttachmentRoleId").IsRequired();

            deviceRoleAttachmentRoleConfiguration.HasOne<DeviceRole>()
                .WithOne()
                .HasForeignKey("DeviceRoleId")
                .IsRequired();

            deviceRoleAttachmentRoleConfiguration.HasOne<AttachmentRole>()
                .WithOne()
                .HasForeignKey("AttachmentRoleId")
                .IsRequired();
        }
    }
}