using Ambev.DeveloperEvaluation.Domain.Products.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Entities;

public class SaleItem
{
    public Product Product { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public bool IsCancelled { get; private set; }

    public decimal DiscountPercentage => GetDiscountPercentage(Quantity);
    public decimal DiscountAmount => UnitPrice * Quantity * DiscountPercentage;
    public decimal Total => IsCancelled ? 0 : UnitPrice * Quantity - DiscountAmount;

    public SaleItem(Product product, int quantity, decimal unitPrice)
    {
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 1)
            throw new InvalidOperationException("Quantity must be at least 1.");

        if (newQuantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 units.");

        Quantity = newQuantity;
    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    private static decimal GetDiscountPercentage(int quantity)
    {
        if (quantity >= 10 && quantity <= 20) return 0.20m;
        if (quantity >= 4 && quantity < 10) return 0.10m;
        return 0.0m;
    }
}

