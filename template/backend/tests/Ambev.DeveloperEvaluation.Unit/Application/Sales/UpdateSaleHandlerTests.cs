using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
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
///     Contains unit tests for the <see cref="UpdateSaleHandler" /> class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly UpdateSaleHandler _handler;
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBroker _rabbitMessageBroker;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UpdateSaleHandlerTests" /> class.
    ///     Sets up test dependencies.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _rabbitMessageBroker = Substitute.For<IMessageBroker>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _rabbitMessageBroker);
    }

    /// <summary>
    ///     Tests that vali sale data and returns success.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var existentSale = new Sale(Customer.CustomerY, Branch.BranchY);

        var expectedResult = new UpdateSaleResult
        {
            Id = existentSale.Id,
            SaleNumber = existentSale.SaleNumber,
            Date = existentSale.Date,
            Customer = command.Customer,
            Branch = command.Branch
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existentSale);

        _mapper.Map(command, existentSale).Returns(existentSale);
        _mapper.Map<UpdateSaleResult>(existentSale).Returns(expectedResult);

        // When
        var updateSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateSaleResult.Should().NotBeNull();
        updateSaleResult.SaleNumber.Should().Be(existentSale.SaleNumber);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        _rabbitMessageBroker.Received(1).PublishEvent(Arg.Any<string>(), Arg.Any<IDomainEvent>());
    }

    /// <summary>
    ///     Tests that valid sale data canceling sale and returns success.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data with label IsCancelled = true When updating sale Then returns success response")]
    public async Task Handle_ValidRequestCancelingSale_ReturnsSuccessResponse()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        command.IsCancelled = true;
        var existentSale = new Sale(Customer.CustomerY, Branch.BranchY);

        var expectedResult = new UpdateSaleResult
        {
            Id = existentSale.Id,
            SaleNumber = existentSale.SaleNumber,
            Date = existentSale.Date,
            Customer = command.Customer,
            Branch = command.Branch
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existentSale);

        _mapper.Map(command, existentSale).Returns(existentSale);
        _mapper.Map<UpdateSaleResult>(existentSale).Returns(expectedResult);

        // When
        var updateSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateSaleResult.Should().NotBeNull();
        updateSaleResult.SaleNumber.Should().Be(existentSale.SaleNumber);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        _rabbitMessageBroker.Received(2).PublishEvent(Arg.Any<string>(), Arg.Any<IDomainEvent>());
    }

    /// <summary>
    ///     Tests that invalid sale data throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When updating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new UpdateSaleCommand(); // Invalid command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    ///     Tests that trying to update a non-existing sale throws an InvalidOperationException.
    /// </summary>
    [Fact(DisplayName = "Given non-existing sale id When updating sale Then throws InvalidOperationException")]
    public async Task Handle_NonExistingSaleId_ThrowsInvalidOperationException()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();

        // Simulate no existing sale found
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale with identifier {command.Id} not found");
    }

    /// <summary>
    ///     Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid command When updating sale Then maps command to sale entity")]
    public async Task Handle_ValidRequest_MapsCommandToSale()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var existentSale = new Sale(Customer.CustomerY, Branch.BranchY);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existentSale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map(command, existentSale);
        await _saleRepository.Received(1).UpdateAsync(existentSale, Arg.Any<CancellationToken>());

        _rabbitMessageBroker.Received(1).PublishEvent(Arg.Any<string>(), Arg.Any<IDomainEvent>());
    }

    /// <summary>
    ///     Tests that attempting to update a cancelled sale throws an InvalidOperationException.
    /// </summary>
    [Fact(DisplayName = "Given a cancelled sale When updating Then throws InvalidOperationException")]
    public async Task Handle_CancelledSale_ThrowsInvalidOperationException()
    {
        // Given
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            Customer = Customer.CustomerY,
            Branch = Branch.BranchZ,
            IsCancelled = false
        };

        var cancelledSale = new Sale(Customer.CustomerX, Branch.BranchZ)
        {
            Id = command.Id,
            IsCancelled = true
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(cancelledSale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Its not possible to update a cancelled sale");
    }
}