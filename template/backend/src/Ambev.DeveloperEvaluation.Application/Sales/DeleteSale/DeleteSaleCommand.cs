using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
///     Command for get sale.
/// </summary>
/// <remarks>
///     This command is used to delete a sale from Id.
///     It implements <see cref="IRequest" /> to initiate the request
///     that returns a <see cref="bool" />.
///     The data provided in this command is validated using the
///     <see cref="DeleteSaleCommandValidator" /> which extends
///     <see cref="AbstractValidator{T}" /> to ensure that the fields are correctly
///     populated and follow the required rules.
/// </remarks>
public class DeleteSaleCommand : IRequest<bool>
{
    /// <summary>
    ///     Unique sale identifier.
    /// </summary>
    public Guid Id { get; set; }
}
