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
                .ForSqlServerUseSequenceHiLo("routinginstanceseq", AttachmentContext.DEFAULT_SCHEMA);                

            routingInstanceConfiguration.Property<string>("Name").IsRequired();
            routingInstanceConfiguration.Property<int>("AdministratorSubField").IsRequired(false);
            routingInstanceConfiguration.Property<int>("AssignedNumberSubField").IsRequired(false);
            routingInstanceConfiguration.Property<int>("DeviceId").IsRequired();

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the collection properties through their fields
            var bgpPeersNavigation = routingInstanceConfiguration.Metadata.FindNavigation(nameof(RoutingInstance.BgpPeers));
            bgpPeersNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
                
        }
    }
}