using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations
{
    class SigmaEntityTypeConfiguration : IEntityTypeConfiguration<Domain.DomainModels.SigmaAggregate.Sigma>
    {
        public void Configure(EntityTypeBuilder<Domain.DomainModels.SigmaAggregate.Sigma> sigmaConfiguration)
        {
            sigmaConfiguration.ToTable("sigma", SigmaContext.DEFAULT_SCHEMA);
            sigmaConfiguration.HasKey(o => o.Id);
            sigmaConfiguration.Ignore(b => b.DomainEvents);

            sigmaConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("sigmaseq", SigmaContext.DEFAULT_SCHEMA);

                                           
        }
    }
}