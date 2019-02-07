using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MINDOnContainers.BuildingBlocks.EventBus.Abstractions;
using MINDOnContainers.BuildingBlocks.EventBus.Events;
using MINDOnContainers.BuildingBlocks.IntegrationEventLogEF;
using MINDOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
using MINDOnContainers.Services.Attachment.Infrastructure;

namespace MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents
{
    public class AttachmentIntegrationEventService : IAttachmentIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly AttachmentContext _attachmentContext;
        private readonly IntegrationEventLogContext _eventLogContext;
        private readonly IIntegrationEventLogService _eventLogService;

        public AttachmentIntegrationEventService(IEventBus eventBus,
            AttachmentContext attachmentContext,
            IntegrationEventLogContext eventLogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _attachmentContext = attachmentContext ?? throw new ArgumentNullException(nameof(attachmentContext));
            _eventLogContext = eventLogContext ?? throw new ArgumentNullException(nameof(eventLogContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_attachmentContext.Database.GetDbConnection());
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
            await _eventLogService.SaveEventAsync(evt, _attachmentContext.GetCurrentTransaction.GetDbTransaction());
        }
    }
}
