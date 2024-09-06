using Boxes.API.Application.Commands.BoxContent;
using Boxes.API.Application.IntegrationEvents;
using Boxes.API.Application.IntegrationEvents.Events;
using Boxes.API.Application.Queries;
using Boxes.API.Extensions;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Infrastructure.Idempotency;
using MediatR;
using MicroBoxer.ServiceDefaults;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Xml.Linq;

namespace Boxes.API.Application.Commands.Box;

public class CreateBoxCommandHandler : IRequestHandler<CreateBoxCommand, BoxDTO>
{
    private readonly IBoxRepository _boxRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateBoxCommandHandler> _logger;
    IBoxesIntegrationEventService _boxIntegrationEventService;


    public CreateBoxCommandHandler(
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

    public async Task<BoxDTO> Handle(CreateBoxCommand command, CancellationToken cancellationToken)
    {

        _logger.LogInformation($"Creating Box from command for User: {command.UserId}");
        var boxContents = new List<Domain.AggregatesModel.BoxAggregate.BoxContent>();
        Domain.AggregatesModel.BoxAggregate.Box box = new Domain.AggregatesModel.BoxAggregate.Box(command.BoxName, command.UserId);
        //{
        //    BoxName = command.BoxName,
        //    BoxContents = boxContents
        //};
        _boxRepository.Add(box);
        await _boxRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return box.ToBoxDTO();
    }


}

// Use for Idempotency in Command process
public class CreateBoxIdentifiedCommandHandler : IdentifiedCommandHandler<CreateBoxCommand, BoxDTO>
{
    public CreateBoxIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<CreateBoxCommand, BoxDTO>> logger)
        : base(mediator, requestManager, logger)
    {
    }

    protected override BoxDTO? CreateResultForDuplicateRequest()
    {
        //return new BoxDTO(new Guid(), "request already in process fake box name"); // Ignore duplicate requests for creating order.
        return null;
    }
}

public record BoxDTO
{
    public Guid Id { get; set; }
    public string BoxName { get; set; }
    //public string BoxType { get; set; }
    public IEnumerable<BoxContentDTO> BoxContents { get; set; }

    public static BoxDTO FromBox(Domain.AggregatesModel.BoxAggregate.Box box)
    {
        return new BoxDTO()
        {
            BoxName = box.BoxName,
            Id = box.Id,
            BoxContents = box.BoxContents.ToBoxContentsDTO()
        };
    }

    private BoxDTO()
    {

    }

    public BoxDTO(Guid Id, string BoxName)
    {
        Id = Id;
        BoxName = BoxName;
        BoxContents = new List<BoxContentDTO>();
    }
    public BoxDTO(Guid Id, string BoxName, List<BoxContentDTO> boxContents)
    {
        Id = Id;
        BoxName = BoxName;
        BoxContents = boxContents;
    }

}
