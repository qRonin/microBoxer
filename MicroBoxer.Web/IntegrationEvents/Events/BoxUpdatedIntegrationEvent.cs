using EventBus.Events;

namespace MicroBoxer.Web.IntegrationEvents
{
    public record BoxUpdatedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public BoxUpdatedIntegrationEvent(Guid id, Guid userId)
        {
            Id = id; UserId = userId;
        }
    }
}
