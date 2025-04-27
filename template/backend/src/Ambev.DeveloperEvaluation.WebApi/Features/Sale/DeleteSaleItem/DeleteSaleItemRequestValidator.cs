using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.DeleteSaleItem;
public class DeleteSaleItemRequestValidator : AbstractValidator<DeleteSaleItemRequest>
{
    /// <summary>
    ///     Validates instances of <see cref="DeleteSaleItemRequest" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Id: Required.
    /// </remarks>
    public DeleteSaleItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required");
    }
}