namespace Ambev.DeveloperEvaluation.Domain.Common.Messaging.Events;
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}