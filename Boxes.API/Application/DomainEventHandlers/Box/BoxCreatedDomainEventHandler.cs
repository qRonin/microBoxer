using Boxes.API.Application.IntegrationEvents;
using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Domain.Events.Box;
using MediatR;

namespace Boxes.API.Application.DomainEventHandlers.Box;

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

        _logger.LogInformation($"Received domain event: {domainEvent.GetType()}, EventContent: {domainEvent.box.Id},{domainEvent.box.BoxName} ");
        var boxCreatedEvent = new BoxCreatedIntegrationEvent
           (domainEvent.box.Id);
        _logger.LogInformation($"Domain event: {domainEvent.GetType()}, Created IntegrationEvent, saving for publish");
        await _boxesIntegrationEventService.AddAndSaveEventAsync(boxCreatedEvent);
        //await _boxesIntegrationEventService.PublishEventsThroughEventBusAsync(boxCreatedEvent);
       

    }
}
