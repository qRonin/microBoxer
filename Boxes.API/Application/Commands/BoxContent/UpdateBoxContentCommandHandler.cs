using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.API.Application.IntegrationEvents;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;

namespace Boxes.API.Application.Commands.BoxContent;

public class UpdateBoxContentCommandHandler : IRequestHandler<UpdateBoxContentCommand, bool>
{
    private readonly IBoxContentRepository _boxContentRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateBoxContentCommandHandler> _logger;
    IBoxesIntegrationEventService _boxIntegrationEventService;

    public UpdateBoxContentCommandHandler(
    IMediator mediator,
    IBoxContentRepository boxContentRepository,
    ILogger<CreateBoxContentCommandHandler> logger,
    IBoxesIntegrationEventService boxIntegrationEventService
    )
    {
        _boxContentRepository = boxContentRepository ?? throw new ArgumentNullException(nameof(boxContentRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _boxIntegrationEventService = boxIntegrationEventService ?? throw new ArgumentNullException(nameof(boxIntegrationEventService));
    }

    public async Task<bool> Handle(UpdateBoxContentCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == null) throw new ArgumentNullException(nameof(command.Id));

        Boxes.Domain.AggregatesModel.BoxAggregate.BoxContent content = await _boxContentRepository.GetAsync((Guid)command.Id);

        content.BoxId = command.BoxId;
        content.Description = command.Description;
        content.LastKnownBoxId = command.BoxId;
        content.Name = command.Name;

        _boxContentRepository.Update(content);
        content.AddBoxContentUpdatedDomainEvent();
        return await _boxContentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
