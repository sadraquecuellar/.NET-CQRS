using Ambev.DeveloperEvaluation.Domain.Common.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;

/// <summary>
///     API response model for Sale items in GetSale operation
/// </summary>
public class GetSaleItemResponse
{
    /// <summary>
    ///     Unique sale item identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     External identifier for the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    ///     Product
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
    ///     Discount applied to the product.
    /// </summary>
    public decimal DiscountAmout { get; set; }

    /// <summary>
    ///     Discount percentage applied to the product.
    /// </summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>
    ///     Total amount for the item after applying discount.
    /// </summary>
    public decimal Total { get; set; }
}