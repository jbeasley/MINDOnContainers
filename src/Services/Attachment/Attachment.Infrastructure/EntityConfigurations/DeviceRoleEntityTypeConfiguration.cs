using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class DeviceRoleEntityTypeConfiguration : IEntityTypeConfiguration<DeviceRole>
    {
        public void Configure(EntityTypeBuilder<DeviceRole> deviceRoleConfiguration)
        {
            deviceRoleConfiguration.ToTable("deviceRole", AttachmentContext.DEFAULT_SCHEMA);
            deviceRoleConfiguration.HasKey(o => o.Id);
            deviceRoleConfiguration.Ignore(b => b.DomainEvents);

            deviceRoleConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("deviceroleseq", AttachmentContext.DEFAULT_SCHEMA);

            deviceRoleConfiguration.Property<bool>("IsProviderDomainRole").IsRequired();

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the collection properties through their fields
            var navigation = deviceRoleConfiguration.Metadata.FindNavigation(nameof(DeviceRole.DeviceRoleAttachmentRoles));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
                            
        }
    }
}