using Boxes.API.Application.Queries;
using MediatR;
using Boxes.API.Infrastructure.Services;

namespace Boxes.API.Apis;

public class BoxesServices(
    IMediator mediator,
    IBoxQueries boxesQueries,
    IIdentityService identityService,
    ILogger<BoxesServices> logger
    , IBoxContentQueries contentQueries
    )
{
    public IMediator Mediator { get; set; } = mediator;
    public ILogger<BoxesServices> Logger { get; } = logger;
    public IBoxQueries BoxesQueries { get; } = boxesQueries;
    public IBoxContentQueries ContentQueries { get; } = contentQueries;
    public IIdentityService IdentityService { get; } = identityService;
}
