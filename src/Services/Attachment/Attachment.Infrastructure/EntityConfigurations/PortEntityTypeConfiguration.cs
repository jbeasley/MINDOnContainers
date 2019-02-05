﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class PortEntityTypeConfiguration : IEntityTypeConfiguration<Port>
    {
        public void Configure(EntityTypeBuilder<Port> portConfiguration)
        {
            portConfiguration.ToTable("port", AttachmentContext.DEFAULT_SCHEMA);
            portConfiguration.HasKey(o => o.Id);
            portConfiguration.Ignore(b => b.DomainEvents);

            portConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("portseq", AttachmentContext.DEFAULT_SCHEMA);
                

            portConfiguration.Property<int>("StatusId").IsRequired();
            portConfiguration.Property<int>("PortBandwidthGbps").IsRequired();
            portConfiguration.Property<int>("PortPoolId").IsRequired();
            portConfiguration.Property<int>("TenantId").IsRequired(false);

            portConfiguration.HasOne<PortStatus>()
                .WithMany()
                .HasForeignKey("PortStatusId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}