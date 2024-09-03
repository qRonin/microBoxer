using Boxes.API.Application.IntegrationEvents;
using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Domain.Events.Box;
using MediatR;

namespace Boxes.API.Application.DomainEventHandlers
{
    public class BoxCreatedDomainEventHandler : INotificationHandler<BoxCreatedDomainEvent>
    {

        private readonly IBoxesIntegrationEventService _boxesIntegrationEventService;
        private readonly ILogger<BoxCreatedDomainEventHandler> _logger;
        private readonly IBoxRepository _boxRepository;
        public BoxCreatedDomainEventHandler(
            IBoxesIntegrationEventService boxesIntegrationEventService,
            ILogger<BoxCreatedDomainEventHandler> logger,
            IBoxRepository boxRepository
            )
        {
            _logger = logger;
            _boxesIntegrationEventService = boxesIntegrationEventService;
            _boxRepository = boxRepository;
        }

        public async Task Handle(BoxCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {

             var boxCreatedEvent = new BoxCreatedIntegrationEvent
                (domainEvent.box.BoxName, domainEvent.box.Id, domainEvent.box.BoxContents);
             await _boxesIntegrationEventService.AddAndSaveEventAsync(boxCreatedEvent);
             await _boxesIntegrationEventService.PublishEventsThroughEventBusAsync(boxCreatedEvent);

        }
    }
}
