using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations
{
    class DeviceStatusEntityTypeConfiguration : IEntityTypeConfiguration<DeviceStatus>
    {
        public void Configure(EntityTypeBuilder<DeviceStatus> deviceStatusConfiguration)
        {
            deviceStatusConfiguration.ToTable("deviceStatus", SigmaContext.DEFAULT_SCHEMA);
            deviceStatusConfiguration.HasKey(o => o.Id);

            deviceStatusConfiguration.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            deviceStatusConfiguration.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();                
        }
    }
}