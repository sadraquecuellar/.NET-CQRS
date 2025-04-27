using Ambev.DeveloperEvaluation.Application.Sale.CreateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;

/// <summary>
///     Command for creating a new sale item.
/// </summary>
/// <remarks>
///     This command is used to capture the required data for creating a sale item,
///     including sale id, product quantity, price and discount.
///     It implements <see cref="IRequest{TResponse}" /> to initiate the request
///     that returns a <see cref="CreateSaleItemResult" />.
///     The data provided in this command is validated using the
///     <see cref="CreateSaleItemCommandValidator" /> which extends
///     <see cref="AbstractValidator{T}" /> to ensure that the fields are correctly
///     populated and follow the required rules.
/// </remarks>
public class CreateSaleItemCommand : IRequest<CreateSaleItemResult>
{
    /// <summary>
    ///     External identifier for the sale.
    /// </summary>
    public Guid SaleId { get; set; }
    /// <summary>
    ///     Quantity of the product purchased.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    ///     Product name (denormalized for better performance).
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    ///     Unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    
}