using Boxes.API.Application.Commands.BoxContent;
using Boxes.API.Application.IntegrationEvents.Events;
using EventBus.Abstractions;
using System.Runtime.Serialization;

namespace Boxes.API.Application.IntegrationEvents.EventHandlers
{
    public class BoxCreatedIntegrationEventHandler : IIntegrationEventHandler<BoxCreatedIntegrationEvent>
    {

        public int Id { get; private set; }
        public string BoxName { get; private set; }
        public List<BoxContentDTO> BoxContents { get; private set; }


        public BoxCreatedIntegrationEventHandler(int id, string boxName)
        {
                Id = id;
                BoxName = boxName;
                BoxContents = new List<BoxContentDTO>();
        }

        public Task Handle(BoxCreatedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
