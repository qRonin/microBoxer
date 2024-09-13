using EventBus.Abstractions;
using MicroBoxer.Web.Services;

namespace MicroBoxer.Web.IntegrationEvents.EventHandlers
{
    public class BoxUpdatedIntegrationEventHandler(
        BoxesNotificationService boxesNotificationService
        ) : IIntegrationEventHandler<BoxUpdatedIntegrationEvent>
    {
        public async Task Handle(BoxUpdatedIntegrationEvent @event)
        {
            await boxesNotificationService.NotifyBoxesChangedAsync(@event.UserId.ToString());
        }
    }
}
