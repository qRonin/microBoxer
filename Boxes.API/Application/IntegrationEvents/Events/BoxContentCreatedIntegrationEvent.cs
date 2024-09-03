using EventBus.Events;

namespace Boxes.API.Application.IntegrationEvents.Events
{
    public record BoxContentCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public BoxContentCreatedIntegrationEvent(Guid id)
        {
                
            Id = id;
        }

    }
}
