using Boxes.API.Application.IntegrationEvents;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;

namespace Boxes.API.Application.Commands.BoxContent;

public class DeleteBoxContentCommandHandler : IRequestHandler<DeleteBoxContentCommand, bool>
{
    private readonly IBoxContentRepository _boxContentRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateBoxContentCommandHandler> _logger;
    IBoxesIntegrationEventService _boxIntegrationEventService;

    public DeleteBoxContentCommandHandler(
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

    public async Task<bool> Handle(DeleteBoxContentCommand request, CancellationToken cancellationToken)
    {
        Boxes.Domain.AggregatesModel.BoxAggregate.BoxContent content = await _boxContentRepository.GetAsync(request.Id);
        _boxContentRepository.Delete(request.Id);
        content.AddBoxContentDeletedDomainEvent();
        return await _boxContentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
