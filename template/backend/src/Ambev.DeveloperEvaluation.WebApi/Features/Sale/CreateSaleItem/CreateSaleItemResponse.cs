namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSaleItem;
/// <summary>
///     API response model for CreateSaleItem operation
/// </summary>
public class CreateSaleItemResponse
{
    /// <summary>
    ///     Sale item identifier
    /// </summary>
    /// <value>A GUID that uniquely identifies the created sale item in the system.</value>
    public Guid Id { get; set; }
}