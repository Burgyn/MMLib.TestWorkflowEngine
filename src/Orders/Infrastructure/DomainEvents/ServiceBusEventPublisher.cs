using Azure.Messaging.ServiceBus;
using Orders.Domain.Events;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Orders.Infrastructure.DomainEvents;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class;
}

public class ServiceBusEventPublisher : IEventPublisher
{
    private readonly ServiceBusClient _client;
    private readonly Dictionary<string, ServiceBusSender> _senders;
    private readonly ILogger<ServiceBusEventPublisher> _logger;

    public ServiceBusEventPublisher(ServiceBusClient client, ILogger<ServiceBusEventPublisher> logger)
    {
        _client = client;
        _logger = logger;
        _senders = new Dictionary<string, ServiceBusSender>
        {
            { nameof(OrderCreatedEvent), _client.CreateSender("order-created") },
            { nameof(OrderStatusChangedEvent), _client.CreateSender("order-status-changed") },
            { nameof(OrderCompletedEvent), _client.CreateSender("order-completed") }
        };
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        var eventType = typeof(T).Name;
        
        _logger.LogInformation("Publishing event: {EventType}", eventType);
        
        if (!_senders.TryGetValue(eventType, out var sender))
        {
            _logger.LogError("Unknown event type: {EventType}", eventType);
            throw new ArgumentException($"Unknown event type: {eventType}");
        }

        var message = new ServiceBusMessage(JsonSerializer.SerializeToUtf8Bytes(@event))
        {
            ContentType = "application/json"
        };

        _logger.LogInformation("Sending message to topic: {TopicName}, Content: {@Event}", 
            sender.EntityPath, @event);

        try 
        {
            await sender.SendMessageAsync(message, cancellationToken);
            _logger.LogInformation("Successfully sent message to topic: {TopicName}", sender.EntityPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send message to topic: {TopicName}", sender.EntityPath);
            throw;
        }
    }

    public void Dispose()
    {
        foreach (var sender in _senders.Values)
        {
            sender.DisposeAsync().AsTask().Wait();
        }
        
        _client.DisposeAsync().AsTask().Wait();
    }
}