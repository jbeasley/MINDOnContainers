using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class DeviceEntityTypeConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> deviceConfiguration)
        {
            deviceConfiguration.ToTable("device", AttachmentContext.DEFAULT_SCHEMA);
            deviceConfiguration.HasKey(o => o.Id);
            deviceConfiguration.Ignore(b => b.DomainEvents);

            deviceConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("deviceseq", AttachmentContext.DEFAULT_SCHEMA);
                

            deviceConfiguration.Property<string>("Name").IsRequired();
            deviceConfiguration.Property<int>("DeviceRoleId").IsRequired();

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the collection properties through their fields
            var routingInstancesNavigation = deviceConfiguration.Metadata.FindNavigation(nameof(Device.RoutingInstances));
            routingInstancesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            var portsNavigation = deviceConfiguration.Metadata.FindNavigation(nameof(Device.Ports));
            portsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            deviceConfiguration.HasOne<DeviceRole>()
                .WithMany()
                .HasForeignKey("DeviceRoleId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}