using Ambev.DeveloperEvaluation.Application.Sales.DeleteSaleItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.DeleteSaleItem;
/// <summary>
///     Profile for mapping between SaleItem entity and their respective representations in aplication layer.
/// </summary>
public class DeleteSaleItemProfile : Profile
{
    /// <summary>
    ///     Initializes the mappings for DeleteSaleItem operation
    /// </summary>
    public DeleteSaleItemProfile()
    {
        CreateMap<DeleteSaleItemRequest, DeleteSaleItemCommand>();
    }
}
