using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;
/// <summary>
///     Profile for mapping between domain entities and their respective representation in Application Layer
/// </summary>
public class UpdateSaleItemProfile : Profile
{
    /// <summary>
    ///     Initializes the mappings for UpdateSaleItem operation
    /// </summary>
    public UpdateSaleItemProfile()
    {
        CreateMap<UpdateSaleItemCommand, SaleItem>();
        CreateMap<SaleItem, UpdateSaleItemResult>();
    }
}