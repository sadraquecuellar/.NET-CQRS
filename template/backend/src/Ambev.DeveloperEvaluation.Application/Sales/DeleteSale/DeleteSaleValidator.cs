using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
/// <summary>
///     Validator for DeleteSaleCommand that defines validation rules for sale deletion command.
/// </summary>
public class DeleteSaleValidator : AbstractValidator<DeleteSaleCommand>
{
    /// <summary>
    ///     Validates instances of <see cref="DeleteSaleCommand" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Id: Required
    /// </remarks>
    public DeleteSaleValidator()
    {
        RuleFor(sale => sale.Id).NotEmpty();
    }
}