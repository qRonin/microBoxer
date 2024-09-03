using EventBus.Events;

namespace Boxes.API.Application.IntegrationEvents
{
    public interface IBoxesIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
        Task PublishEventsThroughEventBusAsync(IntegrationEvent @event);
    }
}
