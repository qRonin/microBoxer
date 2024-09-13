using EventBus.Abstractions;
using MicroBoxer.Web.Services;

namespace MicroBoxer.Web.IntegrationEvents.EventHandlers
{
    public class BoxDeletedIntegrationEventHandler(
        BoxesNotificationService boxesNotificationService
        ) : IIntegrationEventHandler<BoxDeletedIntegrationEvent>
    {
        public async Task Handle(BoxDeletedIntegrationEvent @event)
        {
            await boxesNotificationService.NotifyBoxesChangedAsync(@event.UserId.ToString());
        }
    }
}
