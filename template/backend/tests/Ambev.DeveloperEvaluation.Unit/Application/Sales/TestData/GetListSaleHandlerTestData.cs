using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
/// <summary>
///     Provides methods for generating test data using the Bogus library
/// </summary>
public static class GetListSaleHandlerTestData
{
    /// <summary>
    ///     Generates a fake list of Sale entity with realistic data for testing retrieval
    /// </summary>
    public static List<Sale> GenerateMockSales(int count = 10)
    {
        var saleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                            f.PickRandom(Customer.CustomerZ, Customer.CustomerX, Customer.CustomerY),
                            f.PickRandom(Branch.BranchX, Branch.BranchZ, Branch.BranchY)
                            ))
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
            .RuleFor(s => s.Items, (f, sale) =>
            [
                new(sale.Id, f.PickRandom<Product>(), f.Random.Int(1, 5), f.Finance.Amount())
            ]);

        return saleFaker.Generate(count);
    }
}