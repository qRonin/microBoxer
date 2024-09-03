using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Context.Propagation;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace EventBusRabbitMQ;

public class RabbitMQEventBus(ILogger<RabbitMQEventBus> logger,
        IServiceProvider serviceProvider,
        IOptions<EventBusSubscriptionInfo> subscriptionOptions,
        IOptions<EventBusOptions> options
    ) : IEventBus, IDisposable, IHostedService
{
    private const string ExchangeName = "eventbus";
    //private readonly ResiliencePipeline _pipeline = CreateResiliencePipeline(options.Value.RetryCount);
    //private readonly TextMapPropagator _propagator = rabbitMQTelemetry.Propagator;
    //private readonly ActivitySource _activitySource = rabbitMQTelemetry.ActivitySource;
    private readonly string _queueName = options.Value.SubscriptionClientName;
    private readonly EventBusSubscriptionInfo _subscriptionInfo = subscriptionOptions.Value;
    private IConnection _rabbitMQConnection;

    private static readonly JsonSerializerOptions s_indentedOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions s_caseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };


    private IModel _consumerChannel;



    public void Dispose()
    {
         
    }

    public Task PublishAsync(IntegrationEvent @event)
    {
        var routingKey = @event.GetType().Name;
        using var channel = _rabbitMQConnection?.CreateModel() ?? throw new InvalidOperationException("RabbitMQ connection is not open");
        var body = SerializeMessage(@event);
        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2;
        try
        {
            logger.LogInformation($"Publishing New Event On -  Exchange: {ExchangeName}\n" +
                $"RoutingKey: {routingKey}\n" +
                $"body: {body}\n" +
                $"EventValues: Id:{@event.Id} CreationDate:{@event.CreationDate}");
            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: body);

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"RabbitMQ Starting");

        _ = Task.Factory.StartNew(() =>
        {
            try
            {
                _rabbitMQConnection = serviceProvider.GetRequiredService<IConnection>();
                if (!_rabbitMQConnection.IsOpen)
                {
                    return;
                }
                _consumerChannel = _rabbitMQConnection.CreateModel();
                _consumerChannel.ExchangeDeclare(exchange: ExchangeName,
                            type: "direct");

                _consumerChannel.QueueDeclare(queue: _queueName,
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                logger.LogInformation($"RabbitMQ Started On -  Queue: {_queueName}\n" +
                    $"Exchange: {ExchangeName}\n" +
                    $"");
                consumer.Received += OnMessageReceived;
                _consumerChannel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                    consumer: consumer);

                foreach (var (eventName, _) in _subscriptionInfo.EventTypes)
                {
                    logger.LogInformation($"New SubscriptionEventType: {eventName},\n Creating New QueueBind");
                    _consumerChannel.QueueBind(
                        queue: _queueName,
                        exchange: ExchangeName,
                        routingKey: eventName);
                }

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error starting RabbitMQ connection");
            }
        },
        TaskCreationOptions.LongRunning);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }


    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        logger.LogInformation($"Received Message: {eventArgs.Body} From: {sender.GetType()},\n" +
            $"Exchange: {eventArgs.Exchange}; RoutingKey: {eventArgs.RoutingKey}, PropertiesId: {eventArgs.BasicProperties.MessageId}");


        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
        logger.LogInformation($"MessageValue:{message}, EventType: {eventName}");
        await ProcessEvent(eventName, message);
        _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);

    }
    private async Task ProcessEvent(string eventName, string message)
    {
        await using var scope = serviceProvider.CreateAsyncScope();



        if (!_subscriptionInfo.EventTypes.TryGetValue(eventName, out var eventType))
        {
            logger.LogWarning("Unable to resolve event type for event name {EventName}", eventName);
            return;
        }

        logger.LogInformation($"Deserializing To base IntegrationEvent Attempt: {message}");
        //try
        //{
        //    var deserializedMessage = DeserializeMessage(message, eventType);
        //}
        //catch (Exception ex)
        //{
        //    logger.LogCritical($"Exception occurred: {ex}");
        //}
        var deserializedMessage = DeserializeMessage(message, eventType);
        foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
        {
            logger.LogInformation($"handling each subscriber with message: {deserializedMessage.Id}, {deserializedMessage.CreationDate}");
            await handler.Handle(deserializedMessage);
        }

        await Task.CompletedTask;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026:RequiresUnreferencedCode",
    Justification = "The 'JsonSerializer.IsReflectionEnabledByDefault' feature switch, which is set to false by default for trimmed .NET apps, ensures the JsonSerializer doesn't use Reflection.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode", Justification = "See above.")]
    private IntegrationEvent DeserializeMessage(string message, Type eventType)
    {
        return JsonSerializer.Deserialize(message, eventType, _subscriptionInfo.JsonSerializerOptions) as IntegrationEvent;
    }
    [UnconditionalSuppressMessage("Trimming", "IL2026:RequiresUnreferencedCode",
    Justification = "The 'JsonSerializer.IsReflectionEnabledByDefault' feature switch, which is set to false by default for trimmed .NET apps, ensures the JsonSerializer doesn't use Reflection.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode", Justification = "See above.")]
    private byte[] SerializeMessage(IntegrationEvent @event)
    {
        
          return JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), s_indentedOptions);
        //return JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), _subscriptionInfo.JsonSerializerOptions);
    }

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        // See https://www.pollydocs.org/strategies/retry.html
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
            MaxRetryAttempts = retryCount,
            DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();

        static TimeSpan? GenerateDelay(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }

}
