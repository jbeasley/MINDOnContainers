using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Device.Domain.DomainModels.DeviceAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Device.Infrastructure.EntityConfigurations
{
    class PortStatusEntityTypeConfiguration : IEntityTypeConfiguration<PortStatus>
    {
        public void Configure(EntityTypeBuilder<PortStatus> portStatusConfiguration)
        {
            portStatusConfiguration.ToTable("portStatus", DeviceContext.DEFAULT_SCHEMA);
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