namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;
/// <summary>
///     Represents a request to get a sale in the system.
/// </summary>
public class GetSaleRequest
{
    /// <summary>
    ///     Unique identifier of the sale.
    /// </summary>
    public Guid Id { get; set; }
}