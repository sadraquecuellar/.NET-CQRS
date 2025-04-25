using FluentValidation;
namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
///     Validator for GetSaleCommand that defines validation rules for sale get command.
/// </summary>
public class GetSaleValidator : AbstractValidator<GetSaleCommand>
{
    /// <summary>
    ///     Validates instances of <see cref="GetSaleCommand" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Id: Required
    /// </remarks>
    public GetSaleValidator()
    {
        RuleFor(sale => sale.Id).NotEmpty();
    }
}