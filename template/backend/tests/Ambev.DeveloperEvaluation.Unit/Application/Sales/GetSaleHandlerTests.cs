using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;
/// <summary>
///     Contains unit tests for the <see cref="GetSaleHandler" /> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly GetSaleHandler _handler;
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetSaleHandlerTests" /> class.
    ///     Sets up test dependencies.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid sale id When getting sale Then returns sale result")]
    public async Task Handle_ValidId_ReturnsSaleResult()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var existingSale = GetSaleHandlerTestData.GenerateMockSale();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);

        var expectedResult = new GetSaleResult
        {
            SaleNumber = existingSale.SaleNumber,
            Date = existingSale.Date,
            Customer = existingSale.Customer,
            Branch = existingSale.Branch
        };

        _mapper.Map<GetSaleResult>(existingSale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.SaleNumber.Should().Be(existingSale.SaleNumber);
        await _saleRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
    }

    /// <summary>
    ///     Tests that a non-existing sale id throws an InvalidOperationException.
    /// </summary>
    [Fact(DisplayName = "Given non-existing sale id When getting sale Then throws InvalidOperationException")]
    public async Task Handle_NonExistingId_ThrowsInvalidOperationException()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();

        // Simulate no existing sale found
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }
}