using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;

/// <summary>
///     Provides methods for generating test data using the Bogus library.
/// </summary>
public static class UpdateSaleHandlerTestData
{
    /// <summary>
    ///     Configures the Faker to generate valid UpdateSaleCommand entities.
    ///     The generated sales will have valid:
    ///     - Customer: Valid enum value
    ///     - Branch: Valid enum value
    ///     - Items: At least one valid SaleItem
    /// </summary>
    private static readonly Faker<UpdateSaleCommand> updateSaleCommandFaker = new Faker<UpdateSaleCommand>()
        .RuleFor(s => s.Customer, f => f.PickRandom<Customer>())
        .RuleFor(s => s.Branch, f => f.PickRandom<Branch>());

    /// <summary>
    ///     Generates a valid UpdateSaleCommand entity with randomized data.
    /// </summary>
    /// <returns>A valid UpdateSaleCommand entity with randomly generated data.</returns>
    public static UpdateSaleCommand GenerateValidCommand()
    {
        return updateSaleCommandFaker.Generate();
    }
}