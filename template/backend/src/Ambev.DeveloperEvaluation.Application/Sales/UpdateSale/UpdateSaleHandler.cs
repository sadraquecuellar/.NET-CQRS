using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
///     Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBroker _rabbitMessageBroker;

    /// <summary>
    ///     Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="rabbitMessageBroker">The MessageBroker RabbitMQ</param>
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IMessageBroker rabbitMessageBroker)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _rabbitMessageBroker = rabbitMessageBroker;
    }

    /// <summary>
    ///     Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The Updated sale details</returns>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var existentSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        ValidateSale(command, existentSale);

        _mapper.Map(command, existentSale);

        await _saleRepository.UpdateAsync(existentSale!, cancellationToken);

        if (command.IsCancelled)
            EventNotification(existentSale, true);

        EventNotification(existentSale);

        var result = _mapper.Map<UpdateSaleResult>(existentSale);
        return result;
    }

    /// <summary>
    ///     Check if there is a sale and if a sale was cancelled
    /// </summary>
    private static void ValidateSale(UpdateSaleCommand command, Domain.Sales.Entities.Sale? existentSale)
    {
        if (existentSale == null)
            throw new InvalidOperationException($"Sale with identifier {command.Id} not found");

        if (existentSale.IsCancelled)
            throw new InvalidOperationException("Its not possible to update a cancelled sale");
    }

    /// <summary>
    ///     Send an event notification about a sale updated or cancelled
    /// </summary>
    private void EventNotification(Domain.Sales.Entities.Sale sale, bool isCancelled = false)
    {
        if (isCancelled)
        {
            var saleModifiedEvent = new SaleCancelledEvent
            {
                SaleId = sale.Id,
                SaleNumber = sale.SaleNumber,
                TotalAmount = sale.TotalAmount
            };

            _rabbitMessageBroker.PublishEvent(saleModifiedEvent.ToJsonString(), saleModifiedEvent);
        }
        else {
            var saleModifiedEvent = new SaleModifiedEvent
            {
                SaleId = sale.Id,
                SaleNumber = sale.SaleNumber,
                TotalAmount = sale.TotalAmount
            };

            _rabbitMessageBroker.PublishEvent(saleModifiedEvent.ToJsonString(), saleModifiedEvent);
        }     
    }
}