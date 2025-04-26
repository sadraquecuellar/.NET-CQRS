using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
///     Contains unit tests for the <see cref="DeleteSaleHandler" /> class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly DeleteSaleHandler _handler;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    ///     Initializes a new instance of <see cref="DeleteSaleHandlerTests" /> class.
    ///     Sets up test dependencies.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new DeleteSaleHandler(_saleRepository);
    }

    /// <summary>
    ///     Tests that a valid sale id leads to successful deletion.
    /// </summary>
    [Fact(DisplayName = "Given valid sale id When deleting sale Then returns true")]
    public async Task Handle_ValidId_DeletesSale()
    {
        // Given
        var saleId = Guid.NewGuid();
        _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // When
        var result = await _handler.Handle(new DeleteSaleCommand { Id = saleId }, CancellationToken.None);

        // Then
        result.Should().BeTrue();
        await _saleRepository.Received(1).DeleteAsync(saleId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    ///     Tests that a non-existing sale id returns false.
    /// </summary>
    [Fact(DisplayName = "Given non-existing sale id When deleting sale Then returns false")]
    public async Task Handle_NonExistingId_ReturnsFalse()
    {
        // Given
        var saleId = Guid.NewGuid();
        _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        // When
        var result = await _handler.Handle(new DeleteSaleCommand { Id = saleId }, CancellationToken.None);

        // Then
        result.Should().BeFalse();
        await _saleRepository.Received(1).DeleteAsync(saleId, Arg.Any<CancellationToken>());
    }
}