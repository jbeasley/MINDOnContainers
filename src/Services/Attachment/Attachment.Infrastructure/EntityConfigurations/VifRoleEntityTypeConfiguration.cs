using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate;

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
            vifRoleConfiguration.Property<bool>("RequireRoutingInstance").IsRequired();
            vifRoleConfiguration.Property<int>("IsLayer3Role").IsRequired(false);
            vifRoleConfiguration.Property<int>("RequireContractBandwidth").IsRequired();

            vifRoleConfiguration.HasOne<Domain.DomainModels.AttachmentRoleAggregate.AttachmentRole>()
                .WithMany()
                .IsRequired()
                .HasForeignKey("AttachmentRoleId");
        }
    }
}