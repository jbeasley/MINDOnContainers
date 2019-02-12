using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations
{
    class RoutingInstanceTypeEntityTypeConfiguration : IEntityTypeConfiguration<RoutingInstanceType>
    {
        public void Configure(EntityTypeBuilder<RoutingInstanceType> planeConfiguration)
        {
            planeConfiguration.ToTable("routingInstanceType", SigmaContext.DEFAULT_SCHEMA);
            planeConfiguration.HasKey(o => o.Id);

            planeConfiguration.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            planeConfiguration.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();                
        }
    }
}