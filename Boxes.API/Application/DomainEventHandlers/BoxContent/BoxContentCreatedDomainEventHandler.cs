using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.API.Application.IntegrationEvents;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Domain.Events.Box;
using MediatR;
using Boxes.Domain.Events.BoxContent;

namespace Boxes.API.Application.DomainEventHandlers.BoxContent;

public class BoxContentCreatedDomainEventHandler : INotificationHandler<BoxContentCreatedDomainEvent>
{

    private readonly IBoxesIntegrationEventService _boxesIntegrationEventService;
    private readonly ILogger<BoxContentCreatedDomainEventHandler> _logger;
    private readonly IBoxRepository _boxRepository;
    public BoxContentCreatedDomainEventHandler(
        IBoxesIntegrationEventService boxesIntegrationEventService,
        ILogger<BoxContentCreatedDomainEventHandler> logger,
        IBoxRepository boxRepository
        )
    {
        _logger = logger;
        _boxesIntegrationEventService = boxesIntegrationEventService;
        _boxRepository = boxRepository;
    }

    public async Task Handle(BoxContentCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {


        _logger.LogInformation($"Received domain event: {domainEvent.GetType()}, EventContent: {domainEvent.boxContent.Id},{domainEvent.boxContent.Name} ");
        var boxContentCreatedEvent = new BoxContentCreatedIntegrationEvent
           (domainEvent.boxContent.Id, domainEvent.boxContent.UserId);
        _logger.LogInformation($"Domain event: {domainEvent.GetType()}, Created IntegrationEvent, saving for publish");
        await _boxesIntegrationEventService.AddAndSaveEventAsync(boxContentCreatedEvent);
        //await _boxesIntegrationEventService.PublishEventsThroughEventBusAsync(boxContentCreatedEvent);

    }
}
