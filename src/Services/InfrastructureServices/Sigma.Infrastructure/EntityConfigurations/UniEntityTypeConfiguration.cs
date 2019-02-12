using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations
{
    class UniEntityTypeConfiguration : IEntityTypeConfiguration<Uni>
    {
        public void Configure(EntityTypeBuilder<Uni> uniConfiguration)
        {
            uniConfiguration.ToTable("sigma", SigmaContext.DEFAULT_SCHEMA);
            uniConfiguration.HasKey(o => o.Id);
            uniConfiguration.Ignore(b => b.DomainEvents);

            uniConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("uniseq", SigmaContext.DEFAULT_SCHEMA);

            var portsNavigation = uniConfiguration.Metadata.FindNavigation(nameof(Device.Ports));
            portsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            uniConfiguration.HasOne<Device>()
                .WithMany()
                .HasForeignKey("DeviceId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            uniConfiguration.HasOne<RoutingInstance>()
                .WithMany()
                .HasForeignKey("RoutingInstanceId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}