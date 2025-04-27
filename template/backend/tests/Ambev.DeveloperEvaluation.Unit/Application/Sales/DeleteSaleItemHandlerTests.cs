using Ambev.DeveloperEvaluation.Application.Sales.DeleteSaleItem;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
///     Contains unit tests for the <see cref="DeleteSaleItemHandler" /> class.
/// </summary>
public class DeleteSaleItemHandlerTests
{
    private readonly DeleteSaleItemHandler _handler;
    private readonly ISaleRepository _saleRepository;


    /// <summary>
    ///     Initializes a new instance of <see cref="DeleteSaleItemHandlerTests" /> class.
    ///     Sets up test dependencies.
    /// </summary>
    public DeleteSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new DeleteSaleItemHandler(_saleRepository);
    }

    /// <summary>
    ///     Tests whether a sales item ID is successfully deleted
    /// </summary>
    [Fact(DisplayName = "Given valid sale item id When deleting sale item Then returns true")]
    public async Task Handle_ValidId_DeletesSaleItem()
    {
        // Given
        var saleItemId = Guid.NewGuid();
        _saleRepository.DeleteItemAsync(saleItemId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // When
        var result = await _handler.Handle(new DeleteSaleItemCommand { Id = saleItemId }, CancellationToken.None);

        // Then
        result.Should().BeTrue();
        await _saleRepository.Received(1).DeleteItemAsync(saleItemId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    ///     Tests that a non-existing sale item id returns false.
    /// </summary>
    [Fact(DisplayName = "Given non-existing sale item id When deleting sale item Then returns false")]
    public async Task Handle_NonExistingId_ReturnsFalse()
    {
        // Given
        var saleItemId = Guid.NewGuid();
        _saleRepository.DeleteItemAsync(saleItemId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        // When
        var result = await _handler.Handle(new DeleteSaleItemCommand { Id = saleItemId }, CancellationToken.None);

        // Then
        result.Should().BeFalse();
        await _saleRepository.Received(1).DeleteItemAsync(saleItemId, Arg.Any<CancellationToken>());
    }
}