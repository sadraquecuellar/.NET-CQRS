using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
/// <summary>
///     Handler for processing DeleteSaleCommand requests
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    ///     Initializes a new instance of DeleteSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    public DeleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    ///     Handles the DeleteSaleCommand request
    /// </summary>
    /// <param name="command">The DeleteSale command providing Sale Id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True when object was successfully deleted or false when object not found.</returns>
    public async Task<bool> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        return await _saleRepository.DeleteAsync(command.Id, cancellationToken);
    }
}