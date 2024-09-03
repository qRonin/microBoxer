using EventBus.Events;
using MicroBoxer.Web.Services;

namespace MicroBoxer.Web.IntegrationEvents.Events;

public record BoxCreatedIntegrationEvent : IntegrationEvent
{
    public Guid Id { get; set; }
    public string BoxName { get; init; }

    public IEnumerable<BoxContentRecord> BoxContents { get; set; }

    public BoxCreatedIntegrationEvent(string boxName, Guid id, IEnumerable<BoxContentRecord> boxContents)
    {
        //new DTO here?
        BoxName = boxName;
        Id = id;
        BoxContents = boxContents;

    }

}
