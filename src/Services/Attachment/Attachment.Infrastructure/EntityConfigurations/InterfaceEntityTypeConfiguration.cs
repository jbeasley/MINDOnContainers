using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class InterfaceEntityTypeConfiguration : IEntityTypeConfiguration<Interface>
    {
        public void Configure(EntityTypeBuilder<Interface> interfaceConfiguration)
        {
            interfaceConfiguration.ToTable("interface", AttachmentContext.DEFAULT_SCHEMA);
            interfaceConfiguration.HasKey(o => o.Id);
            interfaceConfiguration.Ignore(b => b.DomainEvents);

            interfaceConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("interfaceseq", AttachmentContext.DEFAULT_SCHEMA);
                
            interfaceConfiguration.Property<int>("Ipv4AddressAndMaskId").IsRequired();

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the collection properties through their fields
            var vlansNavigation = interfaceConfiguration.Metadata.FindNavigation(nameof(Interface.Vlans));
            vlansNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            var portsNavigation = interfaceConfiguration.Metadata.FindNavigation(nameof(Interface.Ports));
            portsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            interfaceConfiguration.HasOne<Ipv4AddressAndMask>()
                .WithOne()
                .HasForeignKey("Ipv4AddressAndMaskId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}