using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.API.Application.IntegrationEvents;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;
using Boxes.API.Extensions;

namespace Boxes.API.Application.Commands.Box;

public class UpdateBoxCommandHandler : IRequestHandler<UpdateBoxCommand, bool>
{
    private readonly IBoxRepository _boxRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateBoxCommandHandler> _logger;
    IBoxesIntegrationEventService _boxIntegrationEventService;


    public UpdateBoxCommandHandler(
        IMediator mediator,
        IBoxRepository boxRepository,
        ILogger<CreateBoxCommandHandler> logger,
        IBoxesIntegrationEventService boxIntegrationEventService
        )
    {
        _boxRepository = boxRepository ?? throw new ArgumentNullException(nameof(boxRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _boxIntegrationEventService = boxIntegrationEventService ?? throw new ArgumentNullException(nameof(boxIntegrationEventService));
    }

    public async Task<bool> Handle(UpdateBoxCommand command, CancellationToken cancellationToken)
    {

        Domain.AggregatesModel.BoxAggregate.Box box = await _boxRepository.GetAsync(command.Id);
        box.BoxContents = command.BoxContents.ToBoxContentsAggregate().ToList();
        box.BoxName = command.BoxName;

        _boxRepository.Update(box);
        box.AddBoxUpdatedDomainEvent();
        return await _boxRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }


}
