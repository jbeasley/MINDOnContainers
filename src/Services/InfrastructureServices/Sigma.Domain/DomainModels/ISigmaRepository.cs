using System.Threading.Tasks;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Sigma Aggregate

    public interface ISigmaRepository : IRepository<Sigma>
    {    
        void Update(Sigma sigma);

        Task<Sigma> GetAsync();
    }
}