using Ambev.DeveloperEvaluation.Domain.Branchs.Entities;
using Ambev.DeveloperEvaluation.Domain.Common.Entities;
using Ambev.DeveloperEvaluation.Domain.Customers.Entities;
using Ambev.DeveloperEvaluation.Domain.Products.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public Customer Customer { get; set; } = default!;
    public Branch Branch { get; set; } = default!;

    private readonly List<SaleItem> _items = [];
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    public decimal TotalAmount => _items.Sum(i => i.Total);
    public bool IsCancelled { get; set; } = false;

    public Sale(string saleNumber, Customer customer, Branch branch)
    {
        SaleNumber = saleNumber;
        Customer = customer;
        Branch = branch;
    }

    public void AddItem(Product product, int quantityToAdd, decimal unitPrice)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Cannot modify a cancelled sale.");

        var existingItem = _items.FirstOrDefault(i => i.Product.Id == product.Id);

        if (existingItem != null)
        {
            int newQuantity = existingItem.Quantity + quantityToAdd;

            if (newQuantity > 20)
                throw new InvalidOperationException("Cannot sell more than 20 units of the same product.");

            existingItem.UpdateQuantity(newQuantity);
            LogEvent("SaleItemUpdated", $"Product {product.Name} quantity updated to {newQuantity}.");
        }
        else
        {
            if (quantityToAdd > 20)
                throw new InvalidOperationException("Cannot sell more than 20 units of the same product.");

            var item = new SaleItem(product, quantityToAdd, unitPrice);
            _items.Add(item);
            LogEvent("SaleItemAdded", $"Product {product.Name} added with quantity {quantityToAdd}.");
        }
    }

    public void CancelItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item == null)
            throw new InvalidOperationException("Item not found in this sale.");

        item.Cancel();
        LogEvent("ItemCancelled", $"Product {item.Product.Name} was cancelled.");
    }

    public void CancelSale()
    {
        if (IsCancelled)
            return;

        IsCancelled = true;
        LogEvent("SaleCancelled", $"Sale {SaleNumber} was cancelled.");
    }

    private static void LogEvent(string eventName, string message)
    {
        Console.WriteLine($"[Event: {eventName}] - {message}");
    }
}

