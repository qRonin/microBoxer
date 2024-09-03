using EventBus.Events;

namespace MicroBoxer.GrpcService.IntegrationEvents.Events;

public record NewIntegrationEvent : IntegrationEvent
{

    public string EventContent { get; set; }
    public NewIntegrationEvent(string content)
    {
        EventContent = content;
    }


}
