using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Configurations;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
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
    }

    public void PublishEvent(string message, IDomainEvent domainEvent)
    {
        var queueName = ToRoutingKey(domainEvent.GetType().Name);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        if (_config.DeclareExchange)
        {
            DeclareExchanges(_channel, _config);
            DeclareDlqQueueAndBindings(_channel, _config, queueName);
        }

        _channel.BasicPublish(
            exchange: _config.Exchange,
            routingKey: "",
            basicProperties: properties,
            body: body
        );
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }

    private static void DeclareExchanges(IModel channel, RabbitMqConfiguration config)
    {
        channel.ExchangeDeclare(
            exchange: config.Exchange, 
            type: config.ExchangeType, 
            durable: true, 
            autoDelete: false
        );
        channel.ExchangeDeclare(
            exchange: $"{config.Exchange}.DLQ",
            type: config.ExchangeType,
            durable: true,
            autoDelete: false
        );
    }
    private static void DeclareDlqQueueAndBindings(IModel channel, RabbitMqConfiguration config, string queueName)
    {
        var primaryQueueArgs = new Dictionary<string, Object>
        {
            { "x-dead-letter-exchange", $"{config.Exchange}.DLQ"},
            { "x-dead-letter-routing-key", ""}
        };

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: primaryQueueArgs
        );
        channel.QueueBind(queueName, config.Exchange, "");

        channel.QueueDeclare(
            queue: $"{queueName}.DLQ",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        channel.QueueBind($"{queueName}.DLQ", $"{config.Exchange}.DLQ", "");
    }

    private static string ToRoutingKey(string eventName)
    {
        var builder = new StringBuilder();
        for (int i = 0; i < eventName.Length; i++)
        {
            char c = eventName[i];
            if (char.IsUpper(c) && i > 0)
                builder.Append('.');

            builder.Append(c);
        }
        return builder.ToString();
    }

}