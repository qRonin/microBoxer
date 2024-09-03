using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

    public static class EventBusbuilderExtensions
    {
        public static IEventBusBuilder ConfigureJsonOptions(this IEventBusBuilder eventBusBuilder, Action<JsonSerializerOptions> configure)
        {
            eventBusBuilder.Services.Configure<EventBusSubscriptionInfo>(o =>
            {
                configure(o.JsonSerializerOptions);
            });

            return eventBusBuilder;
        }

        public static IEventBusBuilder AddSubscription<T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TH>(this IEventBusBuilder eventBusBuilder)
            where T : IntegrationEvent
            where TH : class, IIntegrationEventHandler<T>
        {
            eventBusBuilder.Services.AddKeyedTransient<IIntegrationEventHandler, TH>(typeof(T));
            eventBusBuilder.Services.Configure<EventBusSubscriptionInfo>(o =>
            {
                // Keep track of all registered event types and their name mapping. We send these event types over the message bus
                // and we don't want to do Type.GetType, so we keep track of the name mapping here.

                // This list will also be used to subscribe to events from the underlying message broker implementation.
                o.EventTypes[typeof(T).Name] = typeof(T);
            });


            return eventBusBuilder;
        }

    }

