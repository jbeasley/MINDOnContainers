using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations
{
    class RoutingInstanceEntityTypeConfiguration : IEntityTypeConfiguration<RoutingInstance>
    {
        public void Configure(EntityTypeBuilder<RoutingInstance> routingInstanceConfiguration)
        {
            routingInstanceConfiguration.ToTable("routingInstance", SigmaContext.DEFAULT_SCHEMA);
            routingInstanceConfiguration.HasKey(o => o.Id);
            routingInstanceConfiguration.Ignore(b => b.DomainEvents);

            routingInstanceConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("routingInstanceseq", SigmaContext.DEFAULT_SCHEMA);


            //Value objects persisted as owned entity type supported since EF Core 2.0
            routingInstanceConfiguration.OwnsOne<RouteDistinguisherRange>("RouteDistinguisherRangeId");

            routingInstanceConfiguration.Property<string>("Name").IsRequired();
            routingInstanceConfiguration.Property<int>("AdministratorSubField").IsRequired(false);
            routingInstanceConfiguration.Property<int>("AssignedNumberSubField").IsRequired(false);
            routingInstanceConfiguration.Property<int>("TenantId").IsRequired(false);
            routingInstanceConfiguration.Property<int>("RoutingInstanceTypeId").IsRequired();
            routingInstanceConfiguration.Property<int>("DeviceId").IsRequired();

            routingInstanceConfiguration.HasOne<RoutingInstanceType>()
                .WithMany()
                .HasForeignKey("RoutingInstanceTypeId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}