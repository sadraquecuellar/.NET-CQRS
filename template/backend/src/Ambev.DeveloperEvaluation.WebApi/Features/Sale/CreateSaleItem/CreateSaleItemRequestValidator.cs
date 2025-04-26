using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSaleItem;
/// <summary>
///     Validator for CreateSaleItemRequest that defines validation rules for user creation.
/// </summary>
public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    /// <summary>
    ///     Validates instances of <see cref="CreateSaleItemRequest" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Product: Must be a valid enumeration value.
    ///     - Quantity: Must be greater than zero and less than 20.
    ///     - UnitPrice: Must be greater than or equal to zero.
    /// </remarks>
    public CreateSaleItemRequestValidator()
    {
        RuleFor(item => item.Product).IsInEnum();
        RuleFor(item => item.Quantity).GreaterThan(0).LessThan(20);
        RuleFor(item => item.UnitPrice).GreaterThanOrEqualTo(0);
    }
}