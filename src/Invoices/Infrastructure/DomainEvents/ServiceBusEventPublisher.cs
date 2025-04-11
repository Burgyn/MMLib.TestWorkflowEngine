using Azure.Messaging.ServiceBus;
using Invoices.Domain.Events;
using System.Text.Json;

namespace Invoices.Infrastructure.DomainEvents;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class;
}

public class ServiceBusEventPublisher : IEventPublisher
{
    private readonly ServiceBusClient _client;
    private readonly Dictionary<string, ServiceBusSender> _senders;

    public ServiceBusEventPublisher(ServiceBusClient client)
    {
        _client = client;
        _senders = new Dictionary<string, ServiceBusSender>
        {
            { nameof(InvoiceCreatedEvent), _client.CreateSender("invoice-created") },
            { nameof(InvoicePaidEvent), _client.CreateSender("invoice-paid") }
        };
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        var eventType = typeof(T).Name;

        if (!_senders.TryGetValue(eventType, out var sender))
        {
            throw new ArgumentException($"Unknown event type: {eventType}");
        }

        var message = new ServiceBusMessage(JsonSerializer.SerializeToUtf8Bytes(@event))
        {
            ContentType = "application/json"
        };

        await sender.SendMessageAsync(message, cancellationToken);
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