using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;
public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
{
    /// <summary>
    ///     Validates instances of <see cref="GetSaleRequest" />.
    /// </summary>
    /// <remarks>
    ///     Validation rules include:
    ///     - Id: Required.
    /// </remarks>
    public GetSaleRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required");
    }
}