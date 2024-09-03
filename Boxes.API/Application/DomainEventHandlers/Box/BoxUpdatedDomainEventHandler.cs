using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.API.Application.IntegrationEvents;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Domain.Events.Box;
using MediatR;

namespace Boxes.API.Application.DomainEventHandlers.Box;

public class BoxUpdatedDomainEventHandler : INotificationHandler<BoxUpdatedDomainEvent>
{

    private readonly IBoxesIntegrationEventService _boxesIntegrationEventService;
    private readonly ILogger<BoxUpdatedDomainEventHandler> _logger;
    private readonly IBoxRepository _boxRepository;
    public BoxUpdatedDomainEventHandler(
        IBoxesIntegrationEventService boxesIntegrationEventService,
        ILogger<BoxUpdatedDomainEventHandler> logger,
        IBoxRepository boxRepository
        )
    {
        _logger = logger;
        _boxesIntegrationEventService = boxesIntegrationEventService;
        _boxRepository = boxRepository;
    }

    public async Task Handle(BoxUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {

        var boxCreatedEvent = new BoxUpdatedIntegrationEvent
           (domainEvent.id);
        await _boxesIntegrationEventService.AddAndSaveEventAsync(boxCreatedEvent);
        //await _boxesIntegrationEventService.PublishEventsThroughEventBusAsync(boxCreatedEvent);
        await _boxesIntegrationEventService.PublishEventsThroughEventBusAsync();
    }
}
