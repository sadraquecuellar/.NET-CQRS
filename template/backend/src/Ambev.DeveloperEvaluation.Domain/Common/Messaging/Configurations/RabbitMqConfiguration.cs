namespace Ambev.DeveloperEvaluation.Domain.Common.Messaging.Configurations;
public class RabbitMqConfiguration
{
    public string Exchange { get; set; } = string.Empty;
    public bool DeclareExchange { get; set; }
    public string ExchangeType { get; set; } = "topic";
    public string RoutingKey { get; set; } = string.Empty;
}