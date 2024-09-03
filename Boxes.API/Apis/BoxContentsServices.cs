using Boxes.API.Application.Queries;
using MediatR;

namespace Boxes.API.Apis;

public class BoxContentsServices(
    IMediator mediator,
    IBoxContentQueries queries,
    //IIdentityService identityService,
    ILogger<BoxesServices> logger)
{
    public IMediator Mediator { get; set; } = mediator;
    public ILogger<BoxesServices> Logger { get; } = logger;
    public IBoxContentQueries Queries { get; } = queries;
    //public IIdentityService IdentityService { get; } = identityService;
}
