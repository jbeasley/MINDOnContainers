using MINDOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents
{
    public interface ISigmaIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync();
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}