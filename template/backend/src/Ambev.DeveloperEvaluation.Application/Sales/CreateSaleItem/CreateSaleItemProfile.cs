using Ambev.DeveloperEvaluation.Application.Sale.CreateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;

/// <summary>
///     Profile for mapping between SaleItem and CreateSaleItemResult
/// </summary>
public class CreateSaleItemProfile : Profile
{
    /// <summary>
    ///     Initializes the mappings for CreateSaleItem operation
    /// </summary>
    public CreateSaleItemProfile()
    {
        CreateMap<CreateSaleItemCommand, SaleItem>()
            .ConstructUsing(cmd => new SaleItem(
                cmd.SaleId,
                cmd.Product,
                cmd.Quantity,
                cmd.UnitPrice));

        CreateMap<SaleItem, CreateSaleItemResult>();
    }
}