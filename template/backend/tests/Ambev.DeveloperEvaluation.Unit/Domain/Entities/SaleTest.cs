using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Validations;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
///     Contains unit tests for the Sale entity class.
///     Tests cover cancellation and scenarios of item addition and validation.
/// </summary>
public class SaleTests
{
    /// <summary>
    ///     Tests that when a sale is canceled, it reflects the canceled status.
    /// </summary>
    [Fact(DisplayName = "Sale should be marked as canceled when it's cancelled")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.CancelSale();

        // Assert
        sale.IsCancelled.Should().BeTrue();
    }

    /// <summary>
    ///     Tests that discounts are correctly applied based on item quantity.
    /// </summary>
    [Theory(DisplayName = "Correct discount should be applied based on quantity")]
    [InlineData(3, 0)] // No discount
    [InlineData(4, 0.1)] // 10% discount
    [InlineData(10, 0.2)] // 20% discount
    public void Given_ItemQuantity_When_Created_Then_DiscountShouldBeAppliedCorrectly(int quantity,
        decimal expectedDiscountRate)
    {
        // Arrange
        Sale sale = SaleTestData.GenerateValidSale();
        Product product = Product.ProductX;
        decimal unitPrice = 100m;

        // Act
        var saleItem = new SaleItem(sale.Id, product, quantity, unitPrice);
        sale.AddItem(product, quantity, unitPrice);

        // Assert
        var expectedDiscount = unitPrice * expectedDiscountRate * quantity;
        saleItem.DiscountAmount.Should().Be(expectedDiscount);
    }

    /// <summary>
    ///     Tests that the addition of items over the allowed quantity throws an exception.
    /// </summary>
    [Fact(DisplayName = "Creating more than 20 identical items should throw an exception")]
    public void Given_TooManyItems_When_Created_Then_ShouldThrowException()
    {
        // Arrange
        Sale sale = SaleTestData.GenerateValidSale();
        Product product = Product.ProductX;
        decimal unitPrice = 100m;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            new SaleItem(sale.Id, product, 21, unitPrice));
    }

    /// <summary>
    ///     Tests that a sale is valid according to the SaleValidator rules.
    /// </summary>
    [Fact(DisplayName = "Sale validation should pass")]
    public void Given_ValidSale_When_Validated_Then_ShouldBeValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var validator = new SaleValidator();

        // Act
        var result = validator.Validate(sale);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}