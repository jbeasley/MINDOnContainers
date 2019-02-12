using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations
{
    class PortEntityTypeConfiguration : IEntityTypeConfiguration<Port>
    {
        public void Configure(EntityTypeBuilder<Port> portConfiguration)
        {
            portConfiguration.ToTable("port", SigmaContext.DEFAULT_SCHEMA);
            portConfiguration.HasKey(o => o.Id);
            portConfiguration.Ignore(b => b.DomainEvents);

            portConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("portseq", SigmaContext.DEFAULT_SCHEMA);


            portConfiguration.Property<int>("Name").IsRequired();
            portConfiguration.Property<int>("LocalDeviceIdentifier").IsRequired();
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