using Boxes.Infrastructure;
using EventBus.Abstractions;
using EventBus.Events;
using IntegrationEventLogEF.Services;
using System.Diagnostics;

namespace Boxes.API.Application.IntegrationEvents;

public class BoxesIntegrationEventService(
    IEventBus eventBus,
    BoxesContext boxesContext,
    IIntegrationEventLogService integrationEventLogService,
    ILogger<BoxesIntegrationEventService> logger
    ) : IBoxesIntegrationEventService
{
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    private readonly BoxesContext _boxesContext = boxesContext ?? throw new ArgumentNullException(nameof(boxesContext));
    private readonly IIntegrationEventLogService _eventLogService = integrationEventLogService ?? throw new ArgumentNullException(nameof(integrationEventLogService));
    private readonly ILogger<BoxesIntegrationEventService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {

        var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var logEvt in pendingLogEvents)
        {
            _logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);

            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                await _eventBus.PublishAsync(logEvt.IntegrationEvent);
                await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", logEvt.EventId);

                await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
            }
        }

    }
    public async Task PublishEventsThroughEventBusAsync(IntegrationEvent @event)
    {
        var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(@event);

        foreach (var logEvt in pendingLogEvents)
        {
            _logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);

            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                await _eventBus.PublishAsync(logEvt.IntegrationEvent);
                await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", logEvt.EventId);

                await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
            }
        }
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        _logger.LogInformation("Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);
        await _eventLogService.SaveEventAsync(evt
            , _boxesContext.GetCurrentTransaction()
            );
    }

}
