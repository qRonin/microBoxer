using Boxes.API.Application.IntegrationEvents;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;

namespace Boxes.API.Application.Commands.Box
{
    public class DeleteBoxCommandHandler : IRequestHandler<DeleteBoxCommand, bool>
    {
        private readonly IBoxRepository _boxRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateBoxCommandHandler> _logger;
        IBoxesIntegrationEventService _boxIntegrationEventService;

        public DeleteBoxCommandHandler(
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

        public async Task<bool> Handle(DeleteBoxCommand request, CancellationToken cancellationToken)
        {
            Boxes.Domain.AggregatesModel.BoxAggregate.Box box = await _boxRepository.GetAsync(request.Id);
            _boxRepository.Delete(request.Id);
            box.AddBoxDeletedDomainEvent();
            return await _boxRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }


    }
}
