using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations
{
    class DeviceEntityTypeConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> deviceConfiguration)
        {
            deviceConfiguration.ToTable("device", SigmaContext.DEFAULT_SCHEMA);
            deviceConfiguration.HasKey(o => o.Id);
            deviceConfiguration.Ignore(b => b.DomainEvents);

            deviceConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("deviceseq", SigmaContext.DEFAULT_SCHEMA);

            deviceConfiguration.Property<int>("Name").IsRequired();
            deviceConfiguration.Property<int>("LocationId").IsRequired();

            var portsNavigation = deviceConfiguration.Metadata.FindNavigation(nameof(Device.Ports));
            portsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            var routingInstancesNavigation = deviceConfiguration.Metadata.FindNavigation(nameof(Device.RoutingInstances));
            routingInstancesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            deviceConfiguration.HasOne<DeviceStatus>()
                .WithMany()
                .HasForeignKey("DeviceStatusId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            deviceConfiguration.HasOne<Plane>()
                .WithMany()
                .HasForeignKey("PlaneId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}