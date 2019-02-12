namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
