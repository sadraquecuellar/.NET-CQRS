using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;

/// <summary>
///     Represents a request to create a new sale item in the system.
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    ///     Customer
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    ///     Branch
    /// </summary>
    public Branch Branch { get; set; }


    /// <summary>
    ///     List of items included in the sale.
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}