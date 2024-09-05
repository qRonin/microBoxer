using MicroBoxer.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using EventBusRabbitMQ;
using EventBus.Abstractions;
using MicroBoxer.Web.IntegrationEvents.Events;
using MicroBoxer.Web.IntegrationEvents.EventHandlers;
using WebApp.Services;

namespace MicroBoxer.Web.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddRabbitMqEventBus("EventBus")
        .AddEventBusSubscriptions();
        builder.Services.AddHttpForwarderWithServiceDiscovery();
        builder.Services.AddSingleton<BoxesNotificationService>();
        builder.Services.AddScoped<LogOutService>();
        //builder.Services.AddHttpClient<BoxesService>(b => b.BaseAddress = new("http://boxes-api"))
        //.AddApiVersion(1.0);
        builder.Services.AddHttpClient<BoxesService>(b => b.BaseAddress = new("https://localhost:7046"))
        .AddApiVersion(1.0);
        //.AddAuthToken();



    }
    
    public static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        //eventBus.AddSubscription<event, handler>();
        eventBus.AddSubscription <BoxCreatedIntegrationEvent, BoxCreatedIntegrationEventHandler>();

    }

}