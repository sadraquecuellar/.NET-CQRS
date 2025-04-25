using Ambev.DeveloperEvaluation.Common.Helpers;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetListSale;
/// <summary>
///     Profile for mapping between domain entities and their respective representation in Application Layer
/// </summary>
public class GetListSaleProfile : Profile
{
    public GetListSaleProfile()
    {
        CreateMap<Domain.Sales.Entities.Sale, GetListSaleResult>();
        CreateMap<PagedList<Domain.Sales.Entities.Sale>, GetListSaleResult>();
        CreateMap<SaleItem, GetListSaleItemResult>();
    }
}