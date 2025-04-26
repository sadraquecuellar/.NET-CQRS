using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.DeleteSale;
public class DeleteSaleRequestValidator : AbstractValidator<DeleteSaleRequest>
{
    /// <summary>
    ///     Validates instances of <see cref="DeleteSaleRequest" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Id: Required.
    /// </remarks>
    public DeleteSaleRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required");
    }
}