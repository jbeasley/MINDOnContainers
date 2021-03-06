﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class AttachmentEntityTypeConfiguration : IEntityTypeConfiguration<Domain.DomainModels.AttachmentAggregate.Attachment>
    {
        public void Configure(EntityTypeBuilder<Domain.DomainModels.AttachmentAggregate.Attachment> attachmentConfiguration)
        {
            attachmentConfiguration.ToTable("attachment", AttachmentContext.DEFAULT_SCHEMA);
            attachmentConfiguration.HasKey(o => o.Id);
            attachmentConfiguration.Ignore(b => b.DomainEvents);

            attachmentConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("attachmentseq", AttachmentContext.DEFAULT_SCHEMA);
                
            attachmentConfiguration.Property<string>("Name").IsRequired();
            attachmentConfiguration.Property<string>("Description").IsRequired();
            attachmentConfiguration.Property<string>("Notes").IsRequired(false);
            attachmentConfiguration.Property<int>("TenantId").IsRequired(false);
            attachmentConfiguration.Property<int>("AttachmentRoleId").IsRequired();
            attachmentConfiguration.Property<int>("DeviceId").IsRequired();
            attachmentConfiguration.Property<int>("NetworkStatusId").IsRequired();
            attachmentConfiguration.Property<int>("AttachmentStatusId").IsRequired();
            attachmentConfiguration.Property<int>("MtuId").IsRequired();
            attachmentConfiguration.Property<bool>("Created").IsRequired();

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the collection properties through their fields
            var vifsNavigation = attachmentConfiguration.Metadata.FindNavigation(nameof(Domain.DomainModels.AttachmentAggregate.Attachment.Vifs));
            vifsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            var interfacesNavigation = attachmentConfiguration.Metadata.FindNavigation(nameof(Domain.DomainModels.AttachmentAggregate.Attachment.Interfaces));
            interfacesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            attachmentConfiguration.HasOne<ContractBandwidthPool>()
                .WithMany()
                .HasForeignKey("ContractBandwidthPoolId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            attachmentConfiguration.HasOne<Uni>()
                .WithOne()
                .HasForeignKey("UniId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}