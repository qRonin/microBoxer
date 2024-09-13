using Boxes.API.Application.Commands.BoxContent;
using Boxes.API.Application.IntegrationEvents;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;
using System.Threading;

namespace Boxes.API.Application.Commands.Box;

public class DeleteBoxWithContentCommandHandler : IRequestHandler<DeleteBoxWithContentCommand, bool>
{
    private readonly IBoxRepository _boxRepository;
    private readonly IBoxContentRepository _boxContentRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateBoxCommandHandler> _logger;
    IBoxesIntegrationEventService _boxIntegrationEventService;


    public DeleteBoxWithContentCommandHandler(
        IMediator mediator,
        IBoxRepository boxRepository,
        ILogger<CreateBoxCommandHandler> logger,
        IBoxesIntegrationEventService boxIntegrationEventService,
        IBoxContentRepository boxContentRepository
        )
    {
        _boxContentRepository = boxContentRepository ?? throw new ArgumentNullException(nameof(boxContentRepository));
        _boxRepository = boxRepository ?? throw new ArgumentNullException(nameof(boxRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _boxIntegrationEventService = boxIntegrationEventService ?? throw new ArgumentNullException(nameof(boxIntegrationEventService));
    }

    public async Task<bool> Handle(DeleteBoxWithContentCommand request, CancellationToken cancellationToken)
    {
        Boxes.Domain.AggregatesModel.BoxAggregate.Box box = await _boxRepository.GetAsync(request.Id);
        if (box.BoxContents != null && box.BoxContents.Count > 0)
        {
            /*
            foreach (var content in box.BoxContents)
            {
                //_boxContentRepository.Delete(content.Id);
                //content.AddBoxContentDeletedDomainEvent();


                //DeleteBoxContentCommand deleteBoxContentCommand = new DeleteBoxContentCommand(content.Id);
                //await _mediator.Send(deleteBoxContentCommand, cancellationToken);
            }
            //await _boxContentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            */
            await DeleteContents(box.BoxContents, cancellationToken);
        }

        _boxRepository.Delete(request.Id);
        return await _boxRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }

    private async Task<bool> DeleteContents(IEnumerable<Boxes.Domain.AggregatesModel.BoxAggregate.BoxContent> contents, CancellationToken cancellationToken)
    {

        foreach (var content in contents)
        {
            DeleteBoxContentCommand deleteBoxContentCommand = new DeleteBoxContentCommand(content.Id);
            await _mediator.Send(deleteBoxContentCommand, cancellationToken);
        }
        return await _boxContentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }

}
