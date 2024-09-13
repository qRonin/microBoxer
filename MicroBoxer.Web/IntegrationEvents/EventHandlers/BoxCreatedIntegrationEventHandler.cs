using EventBus.Abstractions;
using MicroBoxer.Web.Services;

namespace MicroBoxer.Web.IntegrationEvents.EventHandlers
{
    public class BoxCreatedIntegrationEventHandler(
        BoxesNotificationService boxesNotificationService
        ) : IIntegrationEventHandler<BoxCreatedIntegrationEvent>
    {
        public async Task Handle(BoxCreatedIntegrationEvent @event)
        {
            await boxesNotificationService.NotifyBoxesChangedAsync(@event.UserId.ToString());

        }
    }
}
