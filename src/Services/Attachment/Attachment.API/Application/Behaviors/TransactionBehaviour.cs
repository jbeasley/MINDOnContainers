﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents;
using MINDOnContainers.Services.Attachment.Infrastructure;

namespace MINDOnContainers.Services.Attachment.API.Infrastructure.Behaviors
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly AttachmentContext _dbContext;
        private readonly IAttachmentIntegrationEventService _orderingIntegrationEventService;

        public TransactionBehaviour(AttachmentContext dbContext,
            IAttachmentIntegrationEventService orderingIntegrationEventService,
            ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(AttachmentContext));
            _orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentException(nameof(orderingIntegrationEventService));
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = default(TResponse);

            try
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () => 
                {
                    _logger.LogInformation($"Begin transaction {typeof(TRequest).Name}");

                    await _dbContext.BeginTransactionAsync();

                    response = await next();

                    await _dbContext.CommitTransactionAsync();

                    _logger.LogInformation($"Committed transaction {typeof(TRequest).Name}");

                    await _orderingIntegrationEventService.PublishEventsThroughEventBusAsync();
                });

                return response;
            }
            catch (Exception)
            {
                _logger.LogInformation($"Rollback transaction executed {typeof(TRequest).Name}");

                _dbContext.RollbackTransaction();
                throw;
            }
        }
    }
}
