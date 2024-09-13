using EventBus.Abstractions;
using MicroBoxer.Web.Services;

namespace MicroBoxer.Web.IntegrationEvents.EventHandlers
{
    public class BoxContentDeletedIntegrationEventHandler(
        BoxesNotificationService boxesNotificationService
        ) : IIntegrationEventHandler<BoxContentDeletedIntegrationEvent>
    {
        public async Task Handle(BoxContentDeletedIntegrationEvent @event)
        {
            await boxesNotificationService.NotifyBoxesChangedAsync(@event.UserId.ToString());

        }
    }
}
