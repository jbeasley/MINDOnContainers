using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MINDOnContainers.BuildingBlocks.EventBus.Abstractions;
using MINDOnContainers.BuildingBlocks.EventBus.Events;
using MINDOnContainers.BuildingBlocks.IntegrationEventLogEF;
using MINDOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents
{
    public class SigmaIntegrationEventService : ISigmaIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly SigmaContext _sigmaContext;
        private readonly IntegrationEventLogContext _eventLogContext;
        private readonly IIntegrationEventLogService _eventLogService;

        public SigmaIntegrationEventService(IEventBus eventBus,
            SigmaContext sigmaContext,
            IntegrationEventLogContext eventLogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _sigmaContext = sigmaContext ?? throw new ArgumentNullException(nameof(sigmaContext));
            _eventLogContext = eventLogContext ?? throw new ArgumentNullException(nameof(eventLogContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_sigmaContext.Database.GetDbConnection());
        }

        public async Task PublishEventsThroughEventBusAsync()
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync();
            foreach (var logEvt in pendingLogEvents)
            {
                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception)
                {
                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            await _eventLogService.SaveEventAsync(evt, _sigmaContext.GetCurrentTransaction.GetDbTransaction());
        }
    }
}
