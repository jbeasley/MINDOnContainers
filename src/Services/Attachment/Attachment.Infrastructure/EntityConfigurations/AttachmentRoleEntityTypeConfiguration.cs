using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;
using MINDOnContainers.Services.Attachment.Infrastructure;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{ 
    class AttachmentRoleEntityTypeConfiguration : IEntityTypeConfiguration<AttachmentRole>
    {
        public void Configure(EntityTypeBuilder<AttachmentRole> attachmentRoleConfiguration)
        {
            attachmentRoleConfiguration.ToTable("attachmentRole", AttachmentContext.DEFAULT_SCHEMA);
            attachmentRoleConfiguration.HasKey(o => o.Id);
            attachmentRoleConfiguration.Ignore(b => b.DomainEvents);

            attachmentRoleConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("attachmentroleseq", AttachmentContext.DEFAULT_SCHEMA);
                

            attachmentRoleConfiguration.Property<string>("Name").IsRequired();
            attachmentRoleConfiguration.Property<bool>("IsLayer3").IsRequired();
            attachmentRoleConfiguration.Property<bool>("IsTaggedRole").IsRequired();
            attachmentRoleConfiguration.Property<bool>("IsTenantFacing").IsRequired();
            attachmentRoleConfiguration.Property<bool>("RequireContractBandwidth").IsRequired();
            attachmentRoleConfiguration.Property<bool>("SupportedByBundle").IsRequired();
            attachmentRoleConfiguration.Property<bool>("SupportedByMultiPort").IsRequired();
            attachmentRoleConfiguration.Property<bool>("RequireRoutingInstance").IsRequired();
            attachmentRoleConfiguration.Property<bool>("RoutingInstanceType").IsRequired();

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the collection properties through their fields
            var vifRolesNavigation = attachmentRoleConfiguration.Metadata.FindNavigation(nameof(AttachmentRole.VifRoles));
            vifRolesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            attachmentRoleConfiguration.HasOne<RoutingInstanceType>()
                .WithMany()
                .HasForeignKey("RoutingInstanceTypeId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}