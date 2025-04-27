using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
///     Validator for UpdateSaleCommand that defines validation rules for user Updation command.
/// </summary>
public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    ///     Validates instances of <see cref="UpdateSaleCommand" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Customer: Must be a valid enumeration value.
    ///     - Branch: Must be a valid enumeration value.
    /// </remarks>
    public UpdateSaleValidator()
    {
        RuleFor(sale => sale.Customer).IsInEnum();
        RuleFor(sale => sale.Branch).IsInEnum();
    }
}