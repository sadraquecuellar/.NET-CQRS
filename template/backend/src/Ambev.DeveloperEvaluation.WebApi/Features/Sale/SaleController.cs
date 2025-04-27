using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.GetListSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Common.Helpers;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetListSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSaleItem;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale;

public class SaleController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    /// <summary>
    ///     Initializes a new instance of SaleController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public SaleController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    ///     Create a new sale
    /// </summary>
    /// <param name="request">The sale creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(response)
        });
    }

    /// <summary>
    ///     Get a sale by Id
    /// </summary>
    /// <param name="id">Unique identifier sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A sale from requested id</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetSaleRequest { Id = id };

        var validator = new GetSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetSaleCommand>(request);

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<GetSaleResponse>
        {
            Success = true,
            Message = "Sale retrieved successfully",
            Data = _mapper.Map<GetSaleResponse>(response)
        });
    }

    /// <summary>
    ///     Get a list of sales with optional filters
    /// </summary>
    /// <param name="request">The request containing optional filters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of sales based on applied filters</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<PagedList<GetListSaleResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetListSale([FromQuery] GetListSaleRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetListSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        var saleResults = _mapper.Map<List<GetListSaleResponse>>(response.Items);

        var result = new PagedList<GetListSaleResponse>
        {
            Items = saleResults,
            Page = response.Page,
            PageSize = response.PageSize,
            TotalCount = response.TotalCount
        };

        return Ok(new ApiResponseWithData<PagedList<GetListSaleResponse>>
        {
            Success = true,
            Message = "Sales retrieved successfully",
            Data = result
        });
    }

    /// <summary>
    ///     Update a sale
    /// </summary>
    /// <param name="id">Unique identifier sale</param>
    /// <param name="request">The sale update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleCommand>(request);

        command.Id = id;

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<UpdateSaleResponse>
        {
            Success = true,
            Message = "Sale updated successfully",
            Data = _mapper.Map<UpdateSaleResponse>(response)
        });
    }

    /// <summary>
    ///     Delete a sale
    /// </summary>
    /// <param name="id">Unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A response indicating the result of the delete operation</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteSaleCommand { Id = id };

        var response = await _mediator.Send(command, cancellationToken);

        if (!response)
            return BadRequest(new ApiResponse { Success = false, Message = "Failed to delete sale, sale not found" });

        return NoContent();
    }

    /// <summary>
    ///     Add a new item to an existing sale
    /// </summary>
    /// <param name="saleId">The ID of the sale to which the item should be added</param>
    /// <param name="request">The request containing item details to be added</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A response indicating the outcome of the operation</returns>
    [HttpPost("{saleId}/item")]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleItemResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSaleItem([FromRoute] Guid saleId, [FromBody] CreateSaleItemRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleItemCommand>(request);
        command.SaleId = saleId;
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleItemResponse>
        {
            Success = true,
            Message = "Item added to sale successfully",
            Data = _mapper.Map<CreateSaleItemResponse>(response)
        });
    }

    /// <summary>
    ///     Update a sale item
    /// </summary>
    /// <param name="id">sale item identifier</param>
    /// <param name="request">The request containing item details to be added</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A response indicating the outcome of the operation</returns>
    [HttpPut("item/{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSaleItem([FromRoute] Guid id, [FromBody] UpdateSaleItemRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleItemCommand>(request);
        command.Id = id;
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<UpdateSaleItemResponse>
        {
            Success = true,
            Message = "Sale Item updated successfully",
            Data = _mapper.Map<UpdateSaleItemResponse>(response)
        });
    }

    /// <summary>
    ///     Delete a sale item
    /// </summary>
    /// <param name="id">Unique identifier of the sale item </param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A response indicating the result of the delete operation</returns>
    [HttpDelete("item/{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteSaleItem([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteSaleItemCommand { Id = id };

        var response = await _mediator.Send(command, cancellationToken);

        if (!response)
            return BadRequest(new ApiResponse
            { Success = false, Message = "Failed to delete sale item, sale item not found" });

        return NoContent();
    }
}