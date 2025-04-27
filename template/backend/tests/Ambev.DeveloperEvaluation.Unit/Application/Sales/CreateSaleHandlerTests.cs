using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Events;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;


/// <summary>
///     Contains unit tests for the <see cref="CreateSaleHandler" /> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly CreateSaleHandler _handler;
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBroker _rabbitMessageBroker;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateSaleHandlerTests" /> class.
    ///     Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _rabbitMessageBroker = Substitute.For<IMessageBroker>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _rabbitMessageBroker);
    }

    /// <summary>
    ///     Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        Sale sale = new(DeveloperEvaluation.Domain.Common.Enums.Customer.CustomerX, DeveloperEvaluation.Domain.Common.Enums.Branch.BranchX);

        sale.Items = command.Items.Select(item => new SaleItem(
            sale.Id,
            item.Product,
            item.Quantity,
            item.UnitPrice)
        ).ToList();

        var result = new CreateSaleResult
        {
            Id = sale.Id
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        _rabbitMessageBroker.Received(1).PublishEvent(Arg.Any<string>(), Arg.Any<IDomainEvent>());
    }

    /// <summary>
    ///     Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    ///     Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to sale entity")]
    public async Task Handle_ValidRequest_MapsCommandToSale()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        Sale sale = new(DeveloperEvaluation.Domain.Common.Enums.Customer.CustomerX, DeveloperEvaluation.Domain.Common.Enums.Branch.BranchX);

        sale.Items = command.Items.Select(item => new SaleItem(
            sale.Id,
            item.Product,
            item.Quantity,
            item.UnitPrice)
        ).ToList();

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        var saleResult = new CreateSaleResult
        {
            Id = sale.Id
        };
        _mapper.Map<CreateSaleResult>(sale).Returns(saleResult);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1)
            .Map<Sale>(Arg.Is<CreateSaleCommand>(c =>
                   c.Customer == command.Customer
                && c.Branch == command.Branch
                && c.Items == command.Items));
        _rabbitMessageBroker.Received(1).PublishEvent(Arg.Any<string>(), Arg.Any<IDomainEvent>());
    }


}