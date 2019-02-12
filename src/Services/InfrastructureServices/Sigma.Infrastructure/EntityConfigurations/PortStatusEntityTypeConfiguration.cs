using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations
{
    class PortStatusEntityTypeConfiguration : IEntityTypeConfiguration<PortStatus>
    {
        public void Configure(EntityTypeBuilder<PortStatus> portStatusConfiguration)
        {
            portStatusConfiguration.ToTable("portStatus", SigmaContext.DEFAULT_SCHEMA);
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