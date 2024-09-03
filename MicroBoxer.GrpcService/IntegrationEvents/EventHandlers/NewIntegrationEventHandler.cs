

using EventBus.Abstractions;
using EventBus.Events;

namespace MicroBoxer.ApiService.Events
{
    public class NewIntegrationEventHandler : IIntegrationEventHandler<IntegrationEvent>
    {
        public Task Handle(IntegrationEvent @event)
        {
            Console.WriteLine(@event);
            return Task.CompletedTask;
        }
    }
}
