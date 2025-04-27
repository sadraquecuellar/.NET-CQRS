using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Events;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class UpdateSaleItemHandlerTests
{
    private readonly UpdateSaleItemHandler _handler;
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBroker _rabbitMessageBroker;

    public UpdateSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _rabbitMessageBroker = Substitute.For<IMessageBroker>();
        _handler = new UpdateSaleItemHandler(_saleRepository, _mapper, _rabbitMessageBroker);
    }

    [Fact(DisplayName = "Given valid command When updating sale item Then item is updated and event is published")]
    public async Task Handle_ValidCommand_UpdatesItemAndPublishesEvents()
    {
        // Given
        var command = UpdateSaleItemHandlerTestData.GenerateValidCommand();
        var existingItem = UpdateSaleItemHandlerTestData.GenerateMockSaleItem(command.Id);

        _saleRepository.GetItemByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingItem);

        // Em vez de mapear para um novo item, mapeia diretamente para o existente
        _mapper.Map(command, existingItem).Returns(existingItem);

        var saleItemResult = new UpdateSaleItemResult
        {
            Id = existingItem.Id,
            SaleId = existingItem.SaleId,
            Product = existingItem.Product,
            Quantity = command.Quantity,
            UnitPrice = command.UnitPrice,
            IsCancelled = existingItem.IsCancelled // Ajuste conforme a lógica de negócio
        };
        _mapper.Map<UpdateSaleItemResult>(existingItem).Returns(saleItemResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeEquivalentTo(saleItemResult);
        await _saleRepository.Received(1).UpdateItemAsync(existingItem, Arg.Any<CancellationToken>());

        _rabbitMessageBroker.Received(1).PublishEvent(Arg.Any<string>(), Arg.Any<IDomainEvent>());
    }

    [Fact(DisplayName = "Given valid command When updating sale item with label IsCancelled = true Then item is updated and event is published")]
    public async Task Handle_ValidCommandWithIsCancelledTrue_UpdatesItemAndPublishesEvents()
    {
        // Given
        var command = UpdateSaleItemHandlerTestData.GenerateValidCommand();
        command.IsCancelled = true;
        var existingItem = UpdateSaleItemHandlerTestData.GenerateMockSaleItem(command.Id);

        _saleRepository.GetItemByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingItem);

        // Em vez de mapear para um novo item, mapeia diretamente para o existente
        _mapper.Map(command, existingItem).Returns(existingItem);

        var saleItemResult = new UpdateSaleItemResult
        {
            Id = existingItem.Id,
            SaleId = existingItem.SaleId,
            Product = existingItem.Product,
            Quantity = command.Quantity,
            UnitPrice = command.UnitPrice,
            IsCancelled = existingItem.IsCancelled // Ajuste conforme a lógica de negócio
        };
        _mapper.Map<UpdateSaleItemResult>(existingItem).Returns(saleItemResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeEquivalentTo(saleItemResult);
        await _saleRepository.Received(1).UpdateItemAsync(existingItem, Arg.Any<CancellationToken>());

        _rabbitMessageBroker.Received(2).PublishEvent(Arg.Any<string>(), Arg.Any<IDomainEvent>());
    }


    [Fact(DisplayName = "Given cancelled sale item When updating Then throws InvalidOperationException")]
    public async Task Handle_CancelledItem_ThrowsInvalidOperationException()
    {
        // Given
        var command = UpdateSaleItemHandlerTestData.GenerateValidCommand();
        var cancelledItem = UpdateSaleItemHandlerTestData.GenerateMockSaleItem(command.Id, true);

        _saleRepository.GetItemByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(cancelledItem);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Its not possible to update a cancelled sale item");
    }
}