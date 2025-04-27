using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
///     Provides methods for generating test data using the Bogus library
/// </summary>
public static class GetSaleHandlerTestData
{
    /// <summary>
    ///     Configures the Faker to generate valid Sale entities.
    /// </summary>
    private static readonly Faker<GetSaleCommand> getSaleCommandFaker = new Faker<GetSaleCommand>()
        .RuleFor(c => c.Id, f => f.Random.Guid());

    /// <summary>
    ///     Generates a valid GetSaleCommand with randomized data.
    /// </summary>
    /// <returns>A valid GetSaleCommand entity with a randomly generated SaleId.</returns>
    public static GetSaleCommand GenerateValidCommand()
    {
        return getSaleCommandFaker.Generate();
    }

    /// <summary>
    ///     Generates a fake Sale entity with realistic data for testing retrieval
    /// </summary>
    /// <returns>A Sale entity populated with valid random data.</returns>
    public static Sale GenerateMockSale()
    {
        var saleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                            f.PickRandom(Customer.CustomerZ, Customer.CustomerX, Customer.CustomerY),
                            f.PickRandom(Branch.BranchX, Branch.BranchZ, Branch.BranchY)
                            ))
            .RuleFor(s => s.Items, (f, sale) =>
            [
                new(sale.Id, f.PickRandom<Product>(), f.Random.Int(1, 5), f.Finance.Amount())
                {
                    Product = f.PickRandom<Product>(),
                    Quantity = f.Random.Int(1, 5),
                    UnitPrice = f.Finance.Amount()
                }
            ]);

        return saleFaker.Generate();
    }
}