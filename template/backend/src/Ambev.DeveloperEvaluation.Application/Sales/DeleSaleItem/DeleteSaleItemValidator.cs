using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleSaleItem;
/// <summary>
///     Validator for DeleteSaleItemCommand that defines validation rules for sale deletion command.
/// </summary>
public class DeleteSaleItemValidator : AbstractValidator<DeleteSaleItemCommand>
{
    /// <summary>
    ///     Validates instances of <see cref="DeleteSaleItemCommand" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Id: Required
    /// </remarks>
    public DeleteSaleItemValidator()
    {
        RuleFor(saleItem => saleItem.Id).NotEmpty();
    }
}