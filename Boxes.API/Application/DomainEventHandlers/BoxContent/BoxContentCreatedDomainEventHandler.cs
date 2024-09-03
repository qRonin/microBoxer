﻿using Boxes.API.Application.IntegrationEvents.Events;
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

        var boxContentCreatedEvent = new BoxContentCreatedIntegrationEvent
           (domainEvent.boxContent.Id);
        await _boxesIntegrationEventService.AddAndSaveEventAsync(boxContentCreatedEvent);
        //await _boxesIntegrationEventService.PublishEventsThroughEventBusAsync(boxContentCreatedEvent);

    }
}
