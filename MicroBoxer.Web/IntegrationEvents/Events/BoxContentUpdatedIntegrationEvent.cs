using EventBus.Events;

namespace MicroBoxer.Web.IntegrationEvents
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
