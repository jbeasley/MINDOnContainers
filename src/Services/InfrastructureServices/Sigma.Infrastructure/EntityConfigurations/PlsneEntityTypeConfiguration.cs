using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations
{
    class PlaneEntityTypeConfiguration : IEntityTypeConfiguration<Plane>
    {
        public void Configure(EntityTypeBuilder<Plane> planeConfiguration)
        {
            planeConfiguration.ToTable("plane", SigmaContext.DEFAULT_SCHEMA);
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