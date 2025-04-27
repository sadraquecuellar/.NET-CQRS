using Ambev.DeveloperEvaluation.Domain.Common.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
///     Represents the response returned after successfully creating a new sale item.
/// </summary>
public class UpdateSaleItemResult
{
    /// <summary>
    ///     Unique identifier of the updated sale item.
    /// </summary>
    /// <value>A GUID that uniquely identifies the updated sale item in the system.</value>
    public Guid Id { get; set; }

    /// <summary>
    ///     External identifier for the sale.
    /// </summary>
    public Guid SaleId { get; set; }

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

    /// <summary>
    ///     Indicates whether the sale item has been canceled.
    /// </summary>
    public bool IsCancelled { get; set; }
}