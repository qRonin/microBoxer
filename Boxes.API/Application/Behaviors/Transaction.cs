using Boxes.API.Application.IntegrationEvents;
using Boxes.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace Boxes.API.Application.Behaviors;

public class Transaction<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<Transaction<TRequest, TResponse>> _logger;
    private readonly BoxesContext _dbContext;
    private readonly IBoxesIntegrationEventService _IntegrationEventService;

    public Transaction(BoxesContext dbContext,
        IBoxesIntegrationEventService integrationEventService,
        ILogger<Transaction<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        _IntegrationEventService = integrationEventService ?? throw new ArgumentException(nameof(integrationEventService));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        //var typeName = 
        try
        {
            if (_dbContext.HasActiveTransaction)
            {
                return await next();
            }

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {

                Guid transactionId;
                await using var transaction = await _dbContext.BeginTransactionAsync();
                response = await next();
                transactionId = transaction.TransactionId;
                _logger.LogWarning($"DB Transaction {transactionId} started");

                await _dbContext.CommitTransactionAsync(transaction, cancellationToken);
                _logger.LogWarning($"DB Transaction {transactionId} commited");
                await _IntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);


            });

            return response;
        }
        catch (Exception ex) {
            _logger.LogError(ex, $"Error Handling transaction for {request.GetType()}");
            throw;
        }
    }
}