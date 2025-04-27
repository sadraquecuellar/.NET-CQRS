using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSaleItem;

/// <summary>
///     Handler for processing DeleteSaleItemCommand requests
/// </summary>
public class DeleteSaleItemHandler : IRequestHandler<DeleteSaleItemCommand, bool>
{
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    ///     Initializes a new instance of DeleteSaleItemHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    public DeleteSaleItemHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    ///     Handles the DeleteSaleItemCommand request
    /// </summary>
    /// <param name="command">The DeleteSaleItem command providing SaleItem Id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True when object was successfully deleted or false when object not found.</returns>
    public async Task<bool> Handle(DeleteSaleItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteSaleItemValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var deleteItemAsync = await _saleRepository.DeleteItemAsync(command.Id, cancellationToken);
        return deleteItemAsync;
    }
}