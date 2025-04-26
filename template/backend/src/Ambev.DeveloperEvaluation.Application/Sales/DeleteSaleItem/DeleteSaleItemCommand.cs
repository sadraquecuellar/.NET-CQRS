using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSaleItem;
/// <summary>
///     Command for delete sale item.
/// </summary>
/// <remarks>
///     This command is used to delete a sale item from Id.
///     It implements <see cref="IRequest" /> to initiate the request
///     that returns a <see cref="bool" />.
///     The data provided in this command is validated using the
///     <see cref="DeleteSaleItemCommandValidator" /> which extends
///     <see cref="AbstractValidator{T}" /> to ensure that the fields are correctly
///     populated and follow the required rules.
/// </remarks>
public class DeleteSaleItemCommand : IRequest<bool>
{
    /// <summary>
    ///     Unique sale item identifier.
    /// </summary>
    public Guid Id { get; set; }
}