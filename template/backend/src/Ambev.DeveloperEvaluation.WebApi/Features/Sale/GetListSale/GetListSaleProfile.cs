using Ambev.DeveloperEvaluation.Application.Sales.GetListSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetListSale;

/// <summary>
///     Profile for mapping between Sale entity and their respective representations in aplication layer.
///     Used to GetListOperation
/// </summary>
public class GetListSaleProfile : Profile
{
    /// <summary>
    ///     Initializes the mappings for GetListSale operation
    /// </summary>
    public GetListSaleProfile()
    {
        CreateMap<GetListSaleRequest, GetListSaleCommand>();
        CreateMap<GetListSaleResult, GetListSaleResponse>();
        CreateMap<GetListSaleItemResult, GetListSaleItemResponse>();
    }
}