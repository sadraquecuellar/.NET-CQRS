using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Configurations;
using Microsoft.Extensions.Configuration;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Events;

namespace Ambev.DeveloperEvaluation.MessageBroker.RabbitMqMessageBroker;
public class RabbitMqMessageBroker : IMessageBroker, IDisposable
{
    private readonly RabbitMqConfiguration _config;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqMessageBroker(IConfiguration configuration)
    {
        var rabbitMQConnection = configuration.GetConnectionString("RabbitMQ");

        if (rabbitMQConnection is null)
            throw new ArgumentNullException(nameof(rabbitMQConnection), "RabbitMQ ConnectionString is null");

        var rabbitMQConfiguration = configuration.GetSection($"RabbitMQConfiguration");
        if(!rabbitMQConfiguration.Exists())
            throw new ArgumentNullException("Section RabbitMQConfiguration not found!");

        _config = rabbitMQConfiguration.Get<RabbitMqConfiguration>();

        var factory = new ConnectionFactory { Uri = new Uri(rabbitMQConnection) };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        if (_config.DeclareExchange)
        {
            _channel.ExchangeDeclare(exchange: _config.Exchange, type: _config.ExchangeType, durable: true, autoDelete: false);
        }
    }

    public void PublishEvent(IDomainEvent domainEvent)
    {
        var routingKey = ToRoutingKey(domainEvent.GetType().Name);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(domainEvent));

        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _channel.BasicPublish(
            exchange: "events",
            routingKey: routingKey,
            basicProperties: properties,
            body: body
        );
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }

    private static string ToRoutingKey(string eventName)
    {
        if (eventName.EndsWith("Event"))
            eventName = eventName[..^5];
        
        var builder = new StringBuilder();
        for (int i = 0; i < eventName.Length; i++)
        {
            char c = eventName[i];
            if (char.IsUpper(c) && i > 0)
                builder.Append('.');
            
            builder.Append(char.ToLower(c));
        }
        return builder.ToString();
    }
}