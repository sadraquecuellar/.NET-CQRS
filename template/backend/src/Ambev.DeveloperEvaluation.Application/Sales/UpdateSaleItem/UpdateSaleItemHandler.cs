using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
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

    /// <summary>
    ///     Initializes a new instance of UpdateSaleItemHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateSaleItemHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
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
        var result = _mapper.Map<UpdateSaleItemResult>(existent);

        return result;
    }
    private static void ValidateExistentSale(UpdateSaleItemCommand command, SaleItem? item)
    {
        if (item == null) throw new InvalidOperationException($"Sale with ID {command.Id} not found");

        if (item.IsCancelled)
            throw new InvalidOperationException("Its not possible to update a cancelled sale item");
    }

    private static void ValidateUpdateDataSale(SaleItem saleItem)
    {
        saleItem.CalculateDiscount();
        saleItem.IsGreaterThan20();
    }
}