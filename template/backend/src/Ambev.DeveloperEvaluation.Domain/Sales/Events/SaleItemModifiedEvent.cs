using System.Text.Json.Serialization;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Events;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Events;
public class SaleItemModifiedEvent : IDomainEvent
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
    public string ToJsonString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}
