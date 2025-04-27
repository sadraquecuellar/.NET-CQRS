using Ambev.DeveloperEvaluation.Domain.Common.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale;
/// <summary>
///     API response model for UpdateSale operation
/// </summary>
public class UpdateSaleResponse
{
    /// <summary>
    ///     Gets or sets the unique identifier of the updated sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Identifier for sale
    /// </summary>
    public string SaleNumber { get; set; }

    /// <summary>
    ///     Date when the sale was made.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    ///     Customer
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    ///     Branch
    /// </summary>
    public Branch Branch { get; set; }
}