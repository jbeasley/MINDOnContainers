using System.Collections.Generic;
using System.Threading.Tasks;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.RegionAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Sigma Aggregate

    public interface IRegionRepository : IRepository<Region>
    {
        Region Add(Region region);

        void Update(Region region);

        Task<Region> GetAsync(int regionId);

        Task<List<Region>> GetAllAsync();
    }
}