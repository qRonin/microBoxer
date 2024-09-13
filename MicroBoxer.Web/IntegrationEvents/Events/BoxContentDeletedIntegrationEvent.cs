using EventBus.Events;

namespace MicroBoxer.Web.IntegrationEvents
{
    public record BoxContentDeletedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public BoxContentDeletedIntegrationEvent(Guid id, Guid userId)
        {
            Id = id; UserId = userId;
        }
    }
}
