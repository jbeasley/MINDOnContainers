using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class UniEntityTypeConfiguration : IEntityTypeConfiguration<Uni>
    {
        public void Configure(EntityTypeBuilder<Uni> uniConfiguration)
        {
            uniConfiguration.ToTable("uni", AttachmentContext.DEFAULT_SCHEMA);
            uniConfiguration.HasKey(o => o.Id);
            uniConfiguration.Ignore(b => b.DomainEvents);

            uniConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("uniseq", AttachmentContext.DEFAULT_SCHEMA);                

            uniConfiguration.Property<string>("Name").IsRequired();
            uniConfiguration.Property<int>("RoutingInstanceId").IsRequired(false);

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the collection properties through their fields
            var bgpPeersNavigation = uniConfiguration.Metadata.FindNavigation(nameof(Uni.BgpPeers));
            bgpPeersNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
                
        }
    }
}