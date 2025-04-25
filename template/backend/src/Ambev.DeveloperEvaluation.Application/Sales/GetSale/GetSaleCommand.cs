using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;
/// <summary>
///     Command for get sale.
/// </summary>
/// <remarks>
///     This command is used to retrieve a sale from Id.
///     It implements <see cref="IRequest" /> to initiate the request
///     that returns a <see cref="GetSaleResult" />.
///     The data provided in this command is validated using the
///     <see cref="GetSaleCommandValidator" /> which extends
///     <see cref="AbstractValidator{T}" /> to ensure that the fields are correctly
///     populated and follow the required rules.
/// </remarks>
public class GetSaleCommand : IRequest<GetSaleResult>
{
    /// <summary>
    ///     Unique sale identifier.
    /// </summary>
    public Guid Id { get; set; }
}