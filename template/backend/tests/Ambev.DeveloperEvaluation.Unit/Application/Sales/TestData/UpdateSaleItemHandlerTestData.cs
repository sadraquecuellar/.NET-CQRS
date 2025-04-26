using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
/// <summary>
///     Provides methods for generating test data using the Bogus library.
/// </summary>
public static class UpdateSaleItemHandlerTestData
{
    public static UpdateSaleItemCommand GenerateValidCommand()
    {
        return new Faker<UpdateSaleItemCommand>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Product, f => f.PickRandom(Product.ProductX, Product.ProductY, Product.ProductZ))
            .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(x => x.UnitPrice, f => f.Finance.Amount());
    }
    public static SaleItem GenerateMockSaleItem(Guid id, bool isCancelled = false)
    {
        var fakeSaleItem = new SaleItem(
            id,
            Product.ProductY,
            10,
            15.5m
        );
        if (isCancelled)
            fakeSaleItem.Cancel();

        return fakeSaleItem;
    }
}
