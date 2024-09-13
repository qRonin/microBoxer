using EventBus.Abstractions;
using MicroBoxer.Web.Services;

namespace MicroBoxer.Web.IntegrationEvents.EventHandlers
{
    public class BoxContentCreatedIntegrationEventHandler(
        BoxesNotificationService boxesNotificationService
        ) : IIntegrationEventHandler<BoxContentCreatedIntegrationEvent>
    {
        public async Task Handle(BoxContentCreatedIntegrationEvent @event)
        {
            await boxesNotificationService.NotifyBoxesChangedAsync(@event.UserId.ToString());

        }
    }
}
