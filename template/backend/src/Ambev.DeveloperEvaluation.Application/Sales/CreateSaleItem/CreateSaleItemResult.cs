namespace Ambev.DeveloperEvaluation.Application.Sale.CreateSaleItem;

/// <summary>
///     Represents the response returned after successfully creating a new sale item.
/// </summary>
public class CreateSaleItemResult
{
    /// <summary>
    ///     Unique identifier of the newly created sale item.
    /// </summary>
    /// <value>A GUID that uniquely identifies the created sale item</value>
    public Guid Id { get; set; }
}