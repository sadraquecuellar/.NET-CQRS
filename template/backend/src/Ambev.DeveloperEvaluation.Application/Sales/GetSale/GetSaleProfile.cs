using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;
/// <summary>
///     Profile for mapping between Sale and GetSaleResult
/// </summary>
public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<Domain.Sales.Entities.Sale, GetSaleResult>();
        CreateMap<SaleItem, GetSaleItemResult>();
    }
}