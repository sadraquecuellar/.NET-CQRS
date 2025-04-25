using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
///     Profile for mapping between domain entities and their respective representation in Application Layer
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    ///     Initializes the mappings for UpdateSale operation
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleCommand, Domain.Sales.Entities.Sale>();
        CreateMap<Domain.Sales.Entities.Sale, UpdateSaleResult>();
        CreateMap<Domain.Sales.Entities.Sale, Domain.Sales.Entities.Sale>();
    }
}