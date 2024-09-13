using EventBus.Events;

namespace Boxes.API.Application.IntegrationEvents.Events
{
    public record BoxContentUpdatedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public BoxContentUpdatedIntegrationEvent(Guid id, Guid userId)
        {
            Id = id; UserId = userId;
        }
    }
}
