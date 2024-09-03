using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.API.Application.IntegrationEvents;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Domain.Events.BoxContent;
using MediatR;

namespace Boxes.API.Application.DomainEventHandlers.BoxContent;

public class BoxContentDeletedDomainEventHandler : INotificationHandler<BoxContentDeletedDomainEvent>
{

    private readonly IBoxesIntegrationEventService _boxesIntegrationEventService;
    private readonly ILogger<BoxContentDeletedDomainEventHandler> _logger;
    private readonly IBoxRepository _boxRepository;
    public BoxContentDeletedDomainEventHandler(
        IBoxesIntegrationEventService boxesIntegrationEventService,
        ILogger<BoxContentDeletedDomainEventHandler> logger,
        IBoxRepository boxRepository
        )
    {
        _logger = logger;
        _boxesIntegrationEventService = boxesIntegrationEventService;
        _boxRepository = boxRepository;
    }

    public async Task Handle(BoxContentDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {

        var boxContentDeletedEvent = new BoxContentDeletedIntegrationEvent
           (domainEvent.id);
        await _boxesIntegrationEventService.AddAndSaveEventAsync(boxContentDeletedEvent);
        //await _boxesIntegrationEventService.PublishEventsThroughEventBusAsync(boxContentDeletedEvent);
        await _boxesIntegrationEventService.PublishEventsThroughEventBusAsync();
    }
}