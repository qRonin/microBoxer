using EventBus.Events;

namespace Boxes.API.Application.IntegrationEvents.Events
{
    public record BoxUpdatedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public BoxUpdatedIntegrationEvent(Guid id)
        {
            Id = id;
        }
    }
}
