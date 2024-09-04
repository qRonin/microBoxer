using EventBus.Abstractions;
using MicroBoxer.Web.IntegrationEvents.Events;
using MicroBoxer.Web.Services;

namespace MicroBoxer.Web.IntegrationEvents.EventHandlers
{
    public class BoxCreatedIntegrationEventHandler(
        BoxesNotificationService boxesNotificationService
        ) : IIntegrationEventHandler<BoxCreatedIntegrationEvent>
    {
        public async Task Handle(BoxCreatedIntegrationEvent @event)
        {
            string userId = "1";
            await boxesNotificationService.NotifyBoxesChangedAsync(userId);

        }
    }
}
