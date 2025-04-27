using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using System.ComponentModel;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale;

/// <summary>
///     Represents a request to update a sale in the system.
/// </summary>
public class UpdateSaleRequest
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
    ///     Indicates whether the sale has been canceled.
    /// </summary>
    [DefaultValue(false)]
    public bool IsCancelled { get; set; }
}
