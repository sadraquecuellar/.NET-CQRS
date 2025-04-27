using Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
///     Provides methods for generating test data using the Bogus library.
/// </summary>
public static class CreateSaleItemHandlerTestData
{
    /// <summary>
    ///     Generates a valid CreateSaleItemCommand entity with randomized data.
    /// </summary>
    public static CreateSaleItemCommand GenerateValidCommand()
    {
        return new Faker<CreateSaleItemCommand>()
            .RuleFor(c => c.SaleId, f => f.Random.Guid())
            .RuleFor(c => c.Product, f => f.PickRandom<Product>())
            .RuleFor(c => c.Quantity, f => f.Random.Int(1, 19))
            .RuleFor(c => c.UnitPrice, f => f.Finance.Amount());
    }

    /// <summary>
    ///     Generates a mock Sale entity with default data for a valid sale.
    /// </summary>
    public static Sale GenerateMockSale()
    {
        return new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                            f.PickRandom(Customer.CustomerZ, Customer.CustomerX, Customer.CustomerY),
                            f.PickRandom(Branch.BranchX, Branch.BranchZ, Branch.BranchY)
                            ))
            .RuleFor(s => s.IsCancelled, _ => false)
            .RuleFor(s => s.Items, _ => []);
    }
}