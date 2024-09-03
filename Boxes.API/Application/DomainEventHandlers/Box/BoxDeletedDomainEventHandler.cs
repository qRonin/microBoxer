using Boxes.API.Application.IntegrationEvents;
using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Domain.Events.Box;
using MediatR;

namespace Boxes.API.Application.DomainEventHandlers.Box;

public class BoxDeletedDomainEventHandler : INotificationHandler<BoxDeletedDomainEvent>
{

    private readonly IBoxesIntegrationEventService _boxesIntegrationEventService;
    private readonly ILogger<BoxDeletedDomainEventHandler> _logger;
    private readonly IBoxRepository _boxRepository;
    public BoxDeletedDomainEventHandler(
        IBoxesIntegrationEventService boxesIntegrationEventService,
        ILogger<BoxDeletedDomainEventHandler> logger,
        IBoxRepository boxRepository
        )
    {
        _logger = logger;
        _boxesIntegrationEventService = boxesIntegrationEventService;
        _boxRepository = boxRepository;
    }

    public async Task Handle(BoxDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {

        var boxCreatedEvent = new BoxDeletedIntegrationEvent
           (domainEvent.id);
        await _boxesIntegrationEventService.AddAndSaveEventAsync(boxCreatedEvent);
        //await _boxesIntegrationEventService.PublishEventsThroughEventBusAsync(boxCreatedEvent);

    }
}
