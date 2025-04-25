using Ambev.DeveloperEvaluation.Common.Helpers;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetListSale;

/// <summary>
///     Handler for processing GetListSaleCommand requests
/// </summary>
public class GetListSaleHandler : IRequestHandler<GetListSaleCommand, PagedList<GetListSaleResult>>
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    ///     Initializes a new instance of GetListSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetListSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    ///     Handles the GetListSaleCommand request
    /// </summary>
    /// <param name="request">The GetListSale command providing optional filters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The retrieved sale list and their items</returns>
    public async Task<PagedList<GetListSaleResult>> Handle(GetListSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetListAsync(
            request.SaleNumber, request.IsCanceled, request.Branch,
            request.Customer, request.SaleDateFrom, request.SaleDateTo, 
            request.Page, request.PageSize, request.SortBy, request.IsDesc, cancellationToken);

        var saleResults = _mapper.Map<List<GetListSaleResult>>(sale.Items);

        var result = new PagedList<GetListSaleResult>
        {
            Items = saleResults,
            Page = sale.Page,
            PageSize = sale.PageSize,
            TotalCount = sale.TotalCount
        };
        return result;
    }
}