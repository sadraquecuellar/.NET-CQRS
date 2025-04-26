using Ambev.DeveloperEvaluation.Domain.Common.Entities;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Entities;

/// <summary>
///     Represents a sales transaction in the system.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    ///     Constructor to set default parameters.
    /// </summary>
    public Sale(Customer customer, Branch branch)
    {
        Customer = customer;
        Branch = branch;
        CalculateTotalAmount();
    }

    /// <summary>
    ///     Unique sale number.
    /// </summary>
    public string SaleNumber { get; set; } = GenerateSaleNumber();

    /// <summary>
    ///     Date when the sale was made.
    /// </summary>
    public DateTime Date { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     Customer
    /// </summary>
    public Customer Customer { get; set; } = default!;

    /// <summary>
    ///     Branch
    /// </summary>
    public Branch Branch { get; set; } = default!;

    /// <summary>
    ///     List of items included in the sale.
    /// </summary>
    public List<SaleItem> Items = [];

    /// <summary>
    ///     Total Amount of sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    ///     Indicates whether the sale has been canceled.
    /// </summary>
    public bool IsCancelled { get; set; } = false;

    /// <summary>
    ///     Add new item for sale.
    /// </summary>
    public void AddItem(Product product, int quantityToAdd, decimal unitPrice)
    {
        if (IsCancelled)
            throw new InvalidOperationException("Cannot modify a cancelled sale.");

        var existingItem = Items.FirstOrDefault(i => i.Product == product);

        if (existingItem != null)
        {
            int newQuantity = existingItem.Quantity + quantityToAdd;
            existingItem.UpdateQuantity(newQuantity);
        }
        else
        {
            var item = new SaleItem(this.Id, product, quantityToAdd, unitPrice);
            Items.Add(item);
        }
        CalculateTotalAmount();
    }

    /// <summary>
    ///     Cancels an item from sale.
    /// </summary>
    public void CancelItem(Product product)
    {
        var item = Items.FirstOrDefault(i => i.Product == product);
        if (item == null)
            throw new InvalidOperationException("Item not found in this sale.");

        item.Cancel();
    }

    /// <summary>
    ///     Cancels the sale.
    /// </summary>
    public void CancelSale()
    {
        if (IsCancelled)
            return;

        IsCancelled = true;
    }

    /// <summary>
    ///     Calculate total.
    /// </summary>
    public void CalculateTotalAmount()
    {
        TotalAmount = Items.Sum(i => i.Total);
    }

    /// <summary>
    ///     Generate a unique sale number.
    /// </summary>
    private static string GenerateSaleNumber()
    {
        return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

}

