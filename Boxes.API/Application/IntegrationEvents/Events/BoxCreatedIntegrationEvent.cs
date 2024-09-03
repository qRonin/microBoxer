using Boxes.API.Application.Commands;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using EventBus.Events;

namespace Boxes.API.Application.IntegrationEvents.Events
{
    public record BoxCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public string BoxName { get; init; }

        public IEnumerable <BoxContent> BoxContents { get; set; }

        public BoxCreatedIntegrationEvent(string boxName, Guid id, IEnumerable<BoxContent> boxContents)
        {
            //new DTO here?
            BoxName = boxName;
            Id = id;
            BoxContents = boxContents;
            
        }

    }
}
