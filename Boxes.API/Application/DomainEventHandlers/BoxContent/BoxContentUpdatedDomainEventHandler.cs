using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.API.Application.IntegrationEvents;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Domain.Events.BoxContent;
using MediatR;

namespace Boxes.API.Application.DomainEventHandlers.BoxContent;

public class BoxContentUpdatedDomainEventHandler : INotificationHandler<BoxContentUpdatedDomainEvent>
{

    private readonly IBoxesIntegrationEventService _boxesIntegrationEventService;
    private readonly ILogger<BoxContentUpdatedDomainEventHandler> _logger;
    private readonly IBoxRepository _boxRepository;
    public BoxContentUpdatedDomainEventHandler(
        IBoxesIntegrationEventService boxesIntegrationEventService,
        ILogger<BoxContentUpdatedDomainEventHandler> logger,
        IBoxRepository boxRepository
        )
    {
        _logger = logger;
        _boxesIntegrationEventService = boxesIntegrationEventService;
        _boxRepository = boxRepository;
    }

    public async Task Handle(BoxContentUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {

        var boxContentUpdatedEvent = new BoxContentUpdatedIntegrationEvent
           (domainEvent.id);
        await _boxesIntegrationEventService.AddAndSaveEventAsync(boxContentUpdatedEvent);

        
    }
}
