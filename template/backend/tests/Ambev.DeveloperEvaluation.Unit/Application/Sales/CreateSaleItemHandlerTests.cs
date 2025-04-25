using Ambev.DeveloperEvaluation.Application.Sale.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;


/// <summary>
///     Contains unit tests for the <see cref="CreateSaleItemHandler" /> class.
/// </summary>
public class CreateSaleItemHandlerTests
{
    private readonly CreateSaleItemHandler _handler;
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    ///     Initializes a new instance of <see cref="CreateSaleItemHandlerTests" /> class.
    ///     Sets up test dependencies.
    /// </summary>
    public CreateSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleItemHandler(_saleRepository, _mapper);
    }

    /// <summary>
    ///     Tests that adding a valid sale item succeeds.
    /// </summary>
    [Fact(DisplayName = "Given valid sale item when added to sale then returns success")]
    public async Task Handle_ValidRequest_AddsSaleItem()
    {
        // Given
        var command = CreateSaleItemHandlerTestData.GenerateValidCommand();
        var existingSale = CreateSaleItemHandlerTestData.GenerateMockSale();

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(existingSale);

        var expectedSaleItem = new SaleItem
        (
            existingSale.Id,
            command.Product,
            command.Quantity,
            command.UnitPrice
        );

        _mapper.Map<SaleItem>(command).Returns(expectedSaleItem);

        var expectedResult = new CreateSaleItemResult
        {
            Id = Guid.NewGuid()
        };

        _mapper.Map<CreateSaleItemResult>(expectedSaleItem).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeEquivalentTo(expectedResult);
        await _saleRepository.Received(1).UpdateAsync(existingSale, Arg.Any<CancellationToken>());
    }

    /// <summary>
    ///     Tests that adding a sale item to a non-existing sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existing sale id When adding sale item Then throws InvalidOperationException")]
    public async Task Handle_NonExistingSaleId_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateSaleItemHandlerTestData.GenerateValidCommand();

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale with ID {command.SaleId} not found");
    }
}