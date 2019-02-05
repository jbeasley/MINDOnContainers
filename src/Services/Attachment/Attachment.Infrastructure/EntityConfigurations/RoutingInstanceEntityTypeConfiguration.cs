using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class RoutingInstanceEntityTypeConfiguration : IEntityTypeConfiguration<RoutingInstance>
    {
        public void Configure(EntityTypeBuilder<RoutingInstance> routingInstanceConfiguration)
        {
            routingInstanceConfiguration.ToTable("routingInstance", AttachmentContext.DEFAULT_SCHEMA);
            routingInstanceConfiguration.HasKey(o => o.Id);
            routingInstanceConfiguration.Ignore(b => b.DomainEvents);

            routingInstanceConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("routingInstanceseq", AttachmentContext.DEFAULT_SCHEMA);


            //Value objects persisted as owned entity type supported since EF Core 2.0
            routingInstanceConfiguration.OwnsOne<RouteDistinguisherRange>("RouteDistinguisherRangeId");

            routingInstanceConfiguration.Property<string>("Name").IsRequired();
            routingInstanceConfiguration.Property<int>("AdministratorSubField").IsRequired(false);
            routingInstanceConfiguration.Property<int>("AssignedNumberSubField").IsRequired(false);
            routingInstanceConfiguration.Property<int>("TenantId").IsRequired(false);
            routingInstanceConfiguration.Property<int>("RoutingInstanceTypeId").IsRequired();
            routingInstanceConfiguration.Property<int>("DeviceId").IsRequired();

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the collection properties through their fields
            var attachmentsNavigation = routingInstanceConfiguration.Metadata.FindNavigation(nameof(RoutingInstance.Attachments));
            attachmentsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            var vifsNavigation = routingInstanceConfiguration.Metadata.FindNavigation(nameof(RoutingInstance.Vifs));
            vifsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            var bgpPeersNavigation = routingInstanceConfiguration.Metadata.FindNavigation(nameof(RoutingInstance.BgpPeers));
            bgpPeersNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            routingInstanceConfiguration.HasOne<RoutingInstanceType>()
                .WithMany()
                .HasForeignKey("RoutingInstanceTypeId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}