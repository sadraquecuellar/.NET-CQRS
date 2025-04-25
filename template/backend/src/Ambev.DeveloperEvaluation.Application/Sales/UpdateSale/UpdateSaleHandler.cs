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

    /// <summary>
    ///     Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
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

        var result = _mapper.Map<UpdateSaleResult>(existentSale);
        return result;
    }

    private static void ValidateSale(UpdateSaleCommand command, Domain.Sales.Entities.Sale? existentSale)
    {
        if (existentSale == null)
            throw new InvalidOperationException($"Sale with identifier {command.Id} not found");

        if (existentSale.IsCancelled)
            throw new InvalidOperationException("Its not possible to update a cancelled sale");
    }
}