using EventBus.Events;

namespace Boxes.API.Application.IntegrationEvents.Events
{
    public record BoxContentDeletedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public BoxContentDeletedIntegrationEvent(Guid id)
        {
            Id = id;
        }
    }
}
