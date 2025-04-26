using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using System.ComponentModel;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSaleItem;
/// <summary>
///     Represents a request to update a new sale item in the system.
/// </summary>
public class UpdateSaleItemRequest
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

    /// <summary>
    ///     Indicates whether the sale item has been canceled.
    /// </summary>
    [DefaultValue(false)]
    public bool IsCancelled { get; set; }
}