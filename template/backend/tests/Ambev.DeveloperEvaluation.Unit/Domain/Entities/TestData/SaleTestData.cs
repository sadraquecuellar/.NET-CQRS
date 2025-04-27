using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
///     Provides methods for generating test data using the Bogus library.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    ///     Configures the Faker to generate valid Sale entities.
    ///     The generated sales will have valid:
    ///     - Customer ID and name (Enum)
    ///     - Branch ID and name (Enum)
    ///     - Total amount
    ///     - Cancellation status
    ///     - List of sale items
    /// </summary>
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .CustomInstantiator(f => new Sale(
                            f.PickRandom(Customer.CustomerZ, Customer.CustomerX, Customer.CustomerY),
                            f.PickRandom(Branch.BranchX, Branch.BranchZ, Branch.BranchY)
                            ))
        .RuleFor(s => s.IsCancelled, false)
        .RuleFor(s => s.Items, (f, sale) => GenerateSaleItems(sale.Id));

    /// <summary>
    ///     Generates a valid Sale entity with randomized data.
    ///     The generated user will have all properties populated with valid values
    ///     that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateValidSale()
    {
        return SaleFaker.Generate();
    }

    /// <summary>
    ///     Generates a list of SaleItem entities with randomized data.
    /// </summary>
    /// <returns>A list of SaleItem with random data.</returns>
    private static List<SaleItem> GenerateSaleItems(Guid saleId)
    {
        var faker = new Faker<SaleItem>()
            .CustomInstantiator(f => new SaleItem(
                saleId,
                f.PickRandom(Product.ProductY, Product.ProductX, Product.ProductZ),
                f.Random.Number(1, 5),
                f.Random.Decimal(0m, 100m)
            ));

        return faker.Generate(3);
    }
}