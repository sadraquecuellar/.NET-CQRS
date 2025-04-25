using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Validations;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.Customer)
            .IsInEnum().WithMessage("Customer is not valid.");

        RuleFor(sale => sale.Branch)
            .IsInEnum().WithMessage("Branch is not valid.");

        RuleFor(sale => sale.Items)
            .NotEmpty().WithMessage("At least one item must be included in the sale.");

        RuleForEach(sale => sale.Items).SetValidator(new SaleItemValidator());
    }
}