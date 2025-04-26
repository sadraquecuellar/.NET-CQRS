using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSaleItem;
/// <summary>
///     Validator for UpdateSaleItemRequest that defines validation rules for user creation.
/// </summary>
public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
{
    /// <summary>
    ///     Validates instances of <see cref="UpdateSaleItemRequest" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Product: Must be a valid enumeration value.
    ///     - Quantity: Must be greater than zero and less than 20.
    ///     - UnitPrice: Must be greater than or equal to zero.
    /// </remarks>
    public UpdateSaleItemRequestValidator()
    {
        RuleFor(item => item.Product).IsInEnum();
        RuleFor(item => item.Quantity).GreaterThan(0).LessThan(20);
        RuleFor(item => item.UnitPrice).GreaterThanOrEqualTo(0);
    }
}