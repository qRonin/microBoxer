using Boxes.API.Application.Commands;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using EventBus.Events;

namespace Boxes.API.Application.IntegrationEvents.Events
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
