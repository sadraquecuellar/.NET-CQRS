using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
///     Command for updating a new sale item.
/// </summary>
/// <remarks>
///     This command is used to capture the required data for updating a sale item,
///     including sale id, product quantity, price and discount.
///     It implements <see cref="IRequest{TResponse}" /> to initiate the request
///     that returns a <see cref="UpdateSaleItemResult" />.
///     The data provided in this command is validated using the
///     <see cref="UpdateSaleItemCommandValidator" /> which extends
///     <see cref="AbstractValidator{T}" /> to ensure that the fields are correctly
///     populated and follow the required rules.
/// </remarks>
public class UpdateSaleItemCommand : IRequest<UpdateSaleItemResult>
{
    /// <summary>
    ///     Sale item unique identifiert
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Product name (denormalized for better performance).
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    ///     Quantity of the product purchased.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    ///     Unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    ///     Indicates whether the sale item has been canceled.
    /// </summary>
    public bool IsCancelled { get; set; }
}