using Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
///     Command for creating a new sale.
/// </summary>
/// <remarks>
///     This command is used to capture the required data for creating a sale,
///     including customer, branch, status cancelled, sale items.
///     It implements <see cref="IRequest" /> to initiate the request
///     that returns a <see cref="CreateSaleResult" />.
///     
///     The data provided in this command is validated using the
///     <see cref="CreateSaleCommandValidator" /> which extends
///     <see cref="AbstractValidator{T}" /> to ensure that the fields are correctly
///     populated and follow the required rules.
/// </remarks>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    ///     Customer name
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    ///     Branch name
    /// </summary>
    public Branch Branch { get; set; }


    /// <summary>
    ///     List of items included in the sale.
    /// </summary>
    public List<CreateSaleItemCommand> Items { get; set; } = [];

    /// <summary>
    ///     Indicates whether the sale has been canceled.
    ///     Default false in sale creation.
    /// </summary>
    public bool IsCancelled { get; set; } = false;
}

