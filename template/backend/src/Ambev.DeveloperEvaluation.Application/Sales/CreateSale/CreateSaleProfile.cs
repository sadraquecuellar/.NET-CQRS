using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
///     Profile for mapping between domain entities and their respective representation in Application Layer
///     Used to create sale operation.
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    ///     Initializes the mappings for CreateSale operation
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Domain.Sales.Entities.Sale>();
        CreateMap<Domain.Sales.Entities.Sale, CreateSaleResult>();
    }
}