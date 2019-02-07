using MINDOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents
{
    public interface IAttachmentIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync();
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}