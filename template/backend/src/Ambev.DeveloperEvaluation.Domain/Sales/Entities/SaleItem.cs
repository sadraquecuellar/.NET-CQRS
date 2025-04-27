using Ambev.DeveloperEvaluation.Domain.Common.Entities;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Entities;

/// <summary>
///     Represents an individual item in a sale transaction.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    ///     Creates a new sale item instance.
    /// </summary>
    public SaleItem(Guid saleId, Product product, int quantity, decimal unitPrice)
    {
        SaleId = saleId;
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
        CalculateDiscount();
        CalculateTotal();
        IsGreaterThan20();
    }

    /// <summary>
    ///     External identifier for the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    ///     Product associated with the sale item.
    /// </summary>
    public Product Product { get;  set; }

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
    public bool IsCancelled { get; private set; } = false;

    /// <summary>
    ///     Percentage discount applied to the product.
    /// </summary>  
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    ///     Discount amount applied to the product.
    /// </summary>  
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    ///     Total amount for the item after applying discount.
    /// </summary>
    public decimal Total { get; private set; }

    /// <summary>
    ///     Update product quantity.
    /// </summary>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 1)
            throw new InvalidOperationException("Quantity must be at least 1.");

        if (newQuantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 units of the same product.");

        Quantity = newQuantity;
    }

    /// <summary>
    ///     Cancels an item from sale.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
    }

    /// <summary>
    ///     Calculates the discount based on the quantity.
    /// </summary>
    public void CalculateDiscount()
    {
        decimal discountRate = Quantity switch
        {
            < 4 => 0m,
            < 10 => 0.10m,
            _ => 0.20m
        };

        DiscountPercentage = discountRate * 100;
        DiscountAmount = UnitPrice * discountRate * Quantity;
    }

    /// <summary>
    ///     Calculate total.
    /// </summary>
    public void CalculateTotal()
    {
        Total = UnitPrice * Quantity - DiscountAmount;
    }

    /// <summary>
    ///    Checks if quantity is greater than 20.
    /// </summary>
    public void IsGreaterThan20()
    {
        if (Quantity > 20) throw new InvalidOperationException("Cannot sell more than 20 units of the same product.");
    }
}

