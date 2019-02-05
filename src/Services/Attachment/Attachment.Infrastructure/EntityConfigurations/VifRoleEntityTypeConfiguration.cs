using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class VifRoleEntityTypeConfiguration : IEntityTypeConfiguration<VifRole>
    {
        public void Configure(EntityTypeBuilder<VifRole> vifRoleConfiguration)
        {
            vifRoleConfiguration.ToTable("vifRole", AttachmentContext.DEFAULT_SCHEMA);
            vifRoleConfiguration.HasKey(o => o.Id);
            vifRoleConfiguration.Ignore(b => b.DomainEvents);

            vifRoleConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("vifroleseq", AttachmentContext.DEFAULT_SCHEMA);

            vifRoleConfiguration.Property<string>("Name").IsRequired();
            vifRoleConfiguration.Property<int>("RoutingInstanceTypeId").IsRequired(false);
            vifRoleConfiguration.Property<bool>("RequireRoutingInstance").IsRequired();
            vifRoleConfiguration.Property<int>("IsLayer3Role").IsRequired(false);
            vifRoleConfiguration.Property<int>("IsTenantFacing").IsRequired();
            vifRoleConfiguration.Property<int>("RequireContractBandwidth").IsRequired();

            vifRoleConfiguration.HasOne<RoutingInstanceType>()
                .WithMany()
                .HasForeignKey("RoutingInstanceTypeId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}