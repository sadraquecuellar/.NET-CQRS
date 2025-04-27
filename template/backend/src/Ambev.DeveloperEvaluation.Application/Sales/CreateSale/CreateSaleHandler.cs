using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using FluentValidation;
using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Sales.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBroker _rabbitMessageBroker;

    /// <summary>
    ///     Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="rabbitMessageBroker">The MessageBroker RabbitMQ</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IMessageBroker rabbitMessageBroker)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _rabbitMessageBroker = rabbitMessageBroker;
    }

    /// <summary>
    ///     Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        var sale = _mapper.Map<Domain.Sales.Entities.Sale>(command);
        sale.CalculateTotalAmount();

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        EventNotification(createdSale);

        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }

    private void EventNotification(Domain.Sales.Entities.Sale sale)
    {
        var saleCreatedEvent = new SaleCreatedEvent
        {
            SaleId = sale.Id,
            SaleNumber = sale.SaleNumber,
            TotalAmount = sale.TotalAmount
        };

        _rabbitMessageBroker.PublishEvent(saleCreatedEvent.ToJsonString(), saleCreatedEvent);
    }
}

