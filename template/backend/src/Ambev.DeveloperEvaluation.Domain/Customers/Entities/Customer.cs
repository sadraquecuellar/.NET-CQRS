using Ambev.DeveloperEvaluation.Domain.Common.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Customers.Entities;

public class Customer: BaseEntity
{
    public string Name { get; set; } = string.Empty;
}

