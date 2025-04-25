using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;

/// <summary>
///     Validator for CreateSaleItemCommand that defines validation rules for sale creation command.
/// </summary>
public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    /// <summary>
    ///     Validates instances of <see cref="CreateSaleItemCommand" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Product: Must be a valid enumeration value.
    ///     - Quantity: Must be greater than zero and less than 20.
    ///     - UnitPrice: Must be greater than or equal to zero.
    /// </remarks>
    public CreateSaleItemCommandValidator()
    {
        RuleFor(item => item.Product).IsInEnum();
        RuleFor(item => item.Quantity).GreaterThan(0).LessThan(20);
        RuleFor(item => item.UnitPrice).GreaterThanOrEqualTo(0);
    }
}