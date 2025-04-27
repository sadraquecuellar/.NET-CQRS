using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;

/// <summary>
///     Handler for processing UpdateSaleItemCommand requests
/// </summary>
public class UpdateSaleItemHandler : IRequestHandler<UpdateSaleItemCommand, UpdateSaleItemResult>
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBroker _rabbitMessageBroker;

    /// <summary>
    ///     Initializes a new instance of UpdateSaleItemHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="rabbitMessageBroker">The MessageBroker RabbitMQ</param>
    public UpdateSaleItemHandler(ISaleRepository saleRepository, IMapper mapper, IMessageBroker rabbitMessageBroker)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _rabbitMessageBroker = rabbitMessageBroker;
    }

    /// <summary>
    ///     Handles the UpdateSaleItemCommand request
    /// </summary>
    /// <param name="command">The UpdateSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    public async Task<UpdateSaleItemResult> Handle(UpdateSaleItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleItemValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var existent = await _saleRepository.GetItemByIdAsync(command.Id, cancellationToken);

        ValidateExistentSale(command, existent);

        _mapper.Map(command, existent);

        ValidateUpdateDataSale(existent!);

        await _saleRepository.UpdateItemAsync(existent!, cancellationToken);

        EventNotification(existent);
        if(command.IsCancelled)
            EventNotification(existent,true);

        var result = _mapper.Map<UpdateSaleItemResult>(existent);

        return result;
    }
    /// <summary>
    ///     Check if there is a sale
    /// </summary>
    private static void ValidateExistentSale(UpdateSaleItemCommand command, SaleItem? item)
    {
        if (item == null) throw new InvalidOperationException($"Sale with ID {command.Id} not found");

        if (item.IsCancelled)
            throw new InvalidOperationException("Its not possible to update a cancelled sale item");
    }

    /// <summary>
    ///     Validate quantity of itens and calcule discount
    /// </summary>
    private static void ValidateUpdateDataSale(SaleItem saleItem)
    {
        saleItem.CalculateDiscount();
        saleItem.IsGreaterThan20();
    }

    /// <summary>
    ///     Send an event notification about a sale item updated or cancelled
    /// </summary>
    private void EventNotification(SaleItem saleItem, bool isCancelled = false)
    {
        if (isCancelled)
        {
            var saleModifiedEvent = new SaleItemCancelledEvent
            {
                Id = saleItem.Id,
                SaleId = saleItem.Id,
            };
            _rabbitMessageBroker.PublishEvent(saleModifiedEvent.ToJsonString(), saleModifiedEvent);
        }
        else
        {
            var saleModifiedEvent = new SaleItemModifiedEvent
            {
                Id = saleItem.Id,
                SaleId = saleItem.SaleId,
            };
            _rabbitMessageBroker.PublishEvent(saleModifiedEvent.ToJsonString(), saleModifiedEvent);
        }
    }
}