using Boxes.API.Application.IntegrationEvents;
using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using EventBus.Abstractions;
using MediatR;

namespace Boxes.API.Application.Commands.BoxContent;

public class CreateBoxContentCommandHandler : IRequestHandler<CreateBoxContentCommand, bool>
{
    private readonly IBoxContentRepository _boxContentRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateBoxContentCommandHandler> _logger;
    IBoxesIntegrationEventService _boxIntegrationEventService;

    public CreateBoxContentCommandHandler(
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

    public async Task<bool> Handle(CreateBoxContentCommand command, CancellationToken cancellationToken)
    {
        Boxes.Domain.AggregatesModel.BoxAggregate.BoxContent content = new Boxes.Domain.AggregatesModel.BoxAggregate.BoxContent()
        {
            BoxId = command.BoxId,
            Description = command.Description,
            LastKnownBoxId = command.BoxId,
            Name = command.Name,
            OrderNumber = 0
        };
        _boxContentRepository.Add(content);
        return await _boxContentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}



public record BoxContentDTO
{
    public Guid? BoxId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //public string ContentType {get; set;}


    public static BoxContentDTO FromBoxContent(Boxes.Domain.AggregatesModel.BoxAggregate.BoxContent content)
    {
        return new BoxContentDTO()
        {
            BoxId = content.BoxId,
            Id = content.Id,
            Name = content.Name,
            Description = content.Description

        };

    }
    public BoxContentDTO()
    {

    }


    public BoxContentDTO(Guid? boxId, Guid boxContentId, string name, string description)
    {
        BoxId = boxId;
        Id = boxContentId;
        Name = name;
        Description = description;

    }


}