using Grpc.Core;
using MicroBoxer.GrpcService.Proto;
using static MicroBoxer.GrpcService.Proto.GrpcService;

namespace MicroBoxer.GrpcService.Grpc;


public class GrpcService(
        //IRepository repository,
        ILogger<GrpcService> logger
    ) : GrpcServiceBase
{
    public override Task<TestEventResponse> GetTestEvent(GetTestEventRequest request, ServerCallContext context)
    {
        //var @event = repository.GetEvent();

        //return MapToTaskEventResponse(@event);

        return base.GetTestEvent(request, context);


    }

    //private static TestEventResponse MapToTaskEventResponse( NewIntegrationEvent @event)
    //{

    //}


}
