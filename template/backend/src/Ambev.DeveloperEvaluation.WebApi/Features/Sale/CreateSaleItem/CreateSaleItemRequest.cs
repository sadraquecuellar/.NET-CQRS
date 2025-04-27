using Ambev.DeveloperEvaluation.Domain.Common.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSaleItem;
/// <summary>
///     Represents a request to create a new sale item in the system.
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    ///     Product.
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    ///     Quantity of the product purchased.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    ///     Unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}