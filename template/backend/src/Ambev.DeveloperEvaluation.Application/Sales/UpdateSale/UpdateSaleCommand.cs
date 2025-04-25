using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
/// <summary>
///     Command for updating a sale.
/// </summary>
/// <remarks>
///     This command is used to capture the required data for updating a sale,
///     including Sale number, sale date, customer, branch, status canceled, sale items.
///     It implements <see cref="IRequest" /> to initiate the request
///     that returns a <see cref="UpdateSaleResult" />.
///     The data provided in this command is validated using the
///     <see cref="UpdateSaleCommandValidator" /> which extends
///     <see cref="AbstractValidator{T}" /> to ensure that the fields are correctly
///     populated and follow the required rules.
/// </remarks>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    ///     Unique identifier of the updated sale.
    /// </summary>
    public Guid Id { get; set; }

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
    public bool IsCancelled { get; set; }
}