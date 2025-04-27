namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.DeleteSale;
/// <summary>
///     Represents a request to delete a sale in the system.
/// </summary>
public class DeleteSaleRequest
{
    /// <summary>
    ///     Unique identifier of the sale.
    /// </summary>
    public Guid Id { get; set; }
}