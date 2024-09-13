using EventBus.Events;

namespace MicroBoxer.Web.IntegrationEvents
{
    public record BoxCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public BoxCreatedIntegrationEvent(Guid id, Guid userId)
        {
            Id = id; UserId = userId;
        }

    }
}
