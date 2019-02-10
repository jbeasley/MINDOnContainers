using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;
using MINDOnContainers.Services.Attachment.Infrastructure;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class VifEntityTypeConfiguration : IEntityTypeConfiguration<Vif>
    {
        public void Configure(EntityTypeBuilder<Vif> vifConfiguration)
        {
            vifConfiguration.ToTable("vif", AttachmentContext.DEFAULT_SCHEMA);
            vifConfiguration.HasKey(o => o.Id);
            vifConfiguration.Ignore(b => b.DomainEvents);

            vifConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("vifseq", AttachmentContext.DEFAULT_SCHEMA);
                

            vifConfiguration.Property<string>("Name").IsRequired();
            vifConfiguration.Property<bool>("IsLayer3").IsRequired();
            vifConfiguration.Property<int>("TenantId").IsRequired(false);
            vifConfiguration.Property<int>("RoutingInstanceId").IsRequired(false);
            vifConfiguration.Property<int>("VifRoleId").IsRequired();
            vifConfiguration.Property<int>("AttachmentId").IsRequired();
            vifConfiguration.Property<int>("NetworkStatusId").IsRequired(false);
            vifConfiguration.Property<int>("MtuId").IsRequired(true);
            vifConfiguration.Property<bool>("Created").IsRequired();

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the collection properties through their fields
            var vifsNavigation = vifConfiguration.Metadata.FindNavigation(nameof(Vif.Vlans));
            vifsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            vifConfiguration.HasOne<Domain.DomainModels.AttachmentAggregate.Attachment>()
                .WithMany()
                .IsRequired()
                .HasForeignKey("AttachmentId");

            vifConfiguration.HasOne<RoutingInstance>()
                .WithMany()
                .HasForeignKey("RoutingInstanceId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            vifConfiguration.HasOne<ContractBandwidthPool>()
                .WithMany()
                .HasForeignKey("ContractBandwidthPoolId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}