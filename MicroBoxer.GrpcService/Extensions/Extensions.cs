using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using EventBusRabbitMQ;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;



namespace MicroBoxer.GrpcService.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddRedisClient("redis");
        //builder.Services.AddSingleton<IRepository, RedisRepository>();
        builder.AddRabbitMqEventBus("eventbus");
            //.AddSubscription<NewIntegrationEvent, NewIntegrationEventHandler>();
            //.ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));
            //.AddSubscriptions();
            


    }


    public static void AddSubscriptions(this IEventBusBuilder eventBusBuilder)
    {
        //eventBusBuilder.AddSubscription<NewIntegrationEvent, NewIntegrationEventHandler>();
    }


}
