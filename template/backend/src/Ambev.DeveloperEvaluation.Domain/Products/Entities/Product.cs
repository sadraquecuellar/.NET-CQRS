using Ambev.DeveloperEvaluation.Domain.Common.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Products.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}

