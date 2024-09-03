using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ;

public static class RabbitMqDependencyInjectionExtensions
{
    private const string SectionName = "EventBus";
    public static IEventBusBuilder AddRabbitMqEventBus(this IHostApplicationBuilder builder, string connectionName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.AddRabbitMQClient(connectionName, configureConnectionFactory: factory =>
        {
            ((ConnectionFactory)factory).DispatchConsumersAsync = true;
        });
        builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection(SectionName));
        builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
        builder.Services.AddSingleton<IHostedService>(sp => (RabbitMQEventBus)sp.GetRequiredService<IEventBus>());
        return new EventBusBuilder(builder.Services);

    }
    private class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
}
