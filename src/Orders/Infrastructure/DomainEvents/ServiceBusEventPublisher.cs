using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace Orders.Infrastructure.DomainEvents;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class;
}

public class ServiceBusEventPublisher : IEventPublisher
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;

    public ServiceBusEventPublisher(ServiceBusClient client)
    {
        _client = client;
        _sender = _client.CreateSender("events");
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        var message = new ServiceBusMessage(JsonSerializer.SerializeToUtf8Bytes(@event))
        {
            Subject = typeof(T).Name,
            ContentType = "application/json"
        };

        await _sender.SendMessageAsync(message, cancellationToken);
    }
} 