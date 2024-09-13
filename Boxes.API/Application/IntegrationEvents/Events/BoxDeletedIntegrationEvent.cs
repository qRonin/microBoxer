using EventBus.Events;

namespace Boxes.API.Application.IntegrationEvents.Events
{
    public record BoxDeletedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public BoxDeletedIntegrationEvent(Guid id, Guid userId)
        {
                Id = id; UserId = userId;
        }
    }
}
