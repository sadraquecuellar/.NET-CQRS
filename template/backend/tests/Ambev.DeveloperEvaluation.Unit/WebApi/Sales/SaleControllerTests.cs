using Ambev.DeveloperEvaluation.Application.Sale.CreateSaleItem;
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
using Ambev.DeveloperEvaluation.WebApi.Features.Sale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetListSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSaleItem;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Sales;
public class SaleControllerTests
{
    private readonly SaleController _controller;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly UpdateSaleRequestValidator _validator;

    public SaleControllerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediator>();
        _validator = Substitute.For<UpdateSaleRequestValidator>(); 
        _controller = new SaleController(_mediator, _mapper);
    }

    [Fact(DisplayName = "CreateSale - Given a valid request, returns a CreatedResponse with status code 201")]
    public async Task CreateSale_ValidRequest_ReturnsCreatedResponse()
    {
        // Given
        var request = new CreateSaleRequest
        {
            Customer = DeveloperEvaluation.Domain.Common.Enums.Customer.CustomerQ,
            Branch = DeveloperEvaluation.Domain.Common.Enums.Branch.BranchQ,
            Items = [
                new(){
                    Product  = DeveloperEvaluation.Domain.Common.Enums.Product.ProductQ,
                    Quantity = 2,
                    UnitPrice = 2
                }
            ]
        };

        var command = new CreateSaleCommand
        {
            Customer = DeveloperEvaluation.Domain.Common.Enums.Customer.CustomerQ,
            Branch = DeveloperEvaluation.Domain.Common.Enums.Branch.BranchQ,
            Items = [
                new(){
                    Product  = DeveloperEvaluation.Domain.Common.Enums.Product.ProductQ,
                    Quantity = 2,
                    UnitPrice = 2
                }
            ]
        };
        
        var resultSale = new CreateSaleResult
        {
            Id = Guid.NewGuid(),
        };

        _mapper.Map<CreateSaleCommand>(request).Returns(command);
        _mediator.Send(Arg.Is<CreateSaleCommand>(c => c == command), Arg.Any<CancellationToken>())
                 .Returns(resultSale);

        // When
        var result = await _controller.CreateSale(request, CancellationToken.None);

        // Then
        var createdResult = result as CreatedResult;
        createdResult.Should().NotBeNull();
        createdResult.StatusCode.Should().Be(201);
    }

    [Fact(DisplayName = "CreateSale - Given an invalid request, returns a BadRequest with status code 400")]
    public async Task CreateSale_InvalidRequest_ReturnsBadRequest()
    {
        // Given
        var request = new CreateSaleRequest();

        var validator = Substitute.For<CreateSaleRequestValidator>();
        validator.ValidateAsync(request, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

        _mapper.Map<CreateSaleCommand>(request).Returns(new CreateSaleCommand());

        // When
        var result = await _controller.CreateSale(request, CancellationToken.None);

        // Then
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName = "GetSale - Given a valid request, returns an OkResponse with status code 200")]
    public async Task GetSale_ValidRequest_ReturnsOkResponse()
    {
        // Given
        var saleId = Guid.NewGuid();
        var request = new GetSaleRequest { Id = saleId };
        var command = new GetSaleCommand { Id = saleId };
        var response = new GetSaleResponse { Id = saleId };

        _mapper.Map<GetSaleCommand>(request).Returns(command);
        _mediator.Send(Arg.Is<GetSaleCommand>(c => c.Id == saleId), Arg.Any<CancellationToken>())
                 .Returns(new GetSaleResult());

        // When
        var result = await _controller.GetSale(saleId, CancellationToken.None);

        // Then
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "GetSale - Given an invalid request, returns a BadRequest with status code 400")]
    public async Task GetSale_InvalidRequest_ReturnsBadRequest()
    {
        // Given
        var invalidId = Guid.Empty;
        var request = new GetSaleRequest { Id = invalidId };
        var validator = Substitute.For<GetSaleRequestValidator>();
        validator.ValidateAsync(request, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

        // When
        var result = await _controller.GetSale(invalidId, CancellationToken.None);

        // Then
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName = "GetListSale - Given a valid request, returns an OkResponse with status code 200")]
    public async Task GetListSale_ValidRequest_ReturnsOkResponse()
    {
        // Given
        var request = new GetListSaleRequest();
        var command = new GetListSaleCommand();
        var response = new GetListSaleResponse();

        _mapper.Map<GetListSaleCommand>(request).Returns(command);
        _mediator.Send(Arg.Is<GetListSaleCommand>(c => c == command), Arg.Any<CancellationToken>())
                 .Returns(new PagedList<GetListSaleResult>());

        // When
        var result = await _controller.GetListSale(request, CancellationToken.None);

        // Then
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200); 
    }

    [Fact(DisplayName = "UpdateSale - Given a valid request, returns an OkResponse with status code 200")]
    public async Task UpdateSale_ValidRequest_ReturnsOkResponse()
    {
        // Given
        var saleId = Guid.NewGuid();
        var request = new UpdateSaleRequest()
        { 
            Branch = DeveloperEvaluation.Domain.Common.Enums.Branch.BranchX,
            Customer = DeveloperEvaluation.Domain.Common.Enums.Customer.CustomerW,
            IsCancelled = false
        };
        var command = new UpdateSaleCommand { Id = saleId };
        var response = new UpdateSaleResult()
        {
            Id = saleId, 
            Branch = DeveloperEvaluation.Domain.Common.Enums.Branch.BranchX,
            Customer = DeveloperEvaluation.Domain.Common.Enums.Customer.CustomerW
        };

        _mapper.Map<UpdateSaleCommand>(request).Returns(command);
        _mediator.Send(Arg.Is<UpdateSaleCommand>(c => c.Id == saleId), Arg.Any<CancellationToken>())
                 .Returns(response);

        // When
        var result = await _controller.UpdateSale(saleId, request, CancellationToken.None);

        // Then
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "UpdateSale - Given an invalid request, returns a BadRequest with status code 400")]
    public async Task UpdateSale_InvalidRequest_ReturnsBadRequest()
    {
        // Given
        var saleId = Guid.NewGuid();
        var request = new UpdateSaleRequest()
        {
            Branch = (DeveloperEvaluation.Domain.Common.Enums.Branch)999,
            Customer = DeveloperEvaluation.Domain.Common.Enums.Customer.CustomerW,
            IsCancelled = false
        };

        var validationResult = new FluentValidation.Results.ValidationResult(
            [
                new FluentValidation.Results.ValidationFailure("Branch", "The Branch value is invalid.")
            ]);

        var validator = Substitute.For<IValidator<UpdateSaleRequest>>();
        validator.ValidateAsync(Arg.Any<UpdateSaleRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(validationResult));

        var validationResultt = await _validator.ValidateAsync(request, CancellationToken.None);

        // When
        var result = await _controller.UpdateSale(saleId, request, CancellationToken.None);

        // Then
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);

        var errors = badRequestResult.Value as IEnumerable<FluentValidation.Results.ValidationFailure>;
        errors.Should().NotBeNull();
        errors.Should().Contain(e => e.PropertyName == "Branch" && e.ErrorMessage.Contains("'Branch' has a range of values which does not include"));
    }

    [Fact(DisplayName = "DeleteSale - Given a valid request, returns a NoContentResponse with status code 204")]
    public async Task DeleteSale_ValidRequest_ReturnsNoContentResponse()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new DeleteSaleCommand { Id = saleId };

        _mediator.Send(Arg.Is<DeleteSaleCommand>(c => c.Id == saleId), Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult(true));

        // When
        var result = await _controller.DeleteSale(saleId, CancellationToken.None);

        // Then
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
    }

    [Fact(DisplayName = "DeleteSale - Given an invalid request, returns a BadRequest with status code 400")]
    public async Task DeleteSale_InvalidRequest_ReturnsBadRequest()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new DeleteSaleCommand { Id = saleId };

        _mediator.Send(Arg.Is<DeleteSaleCommand>(c => c.Id == saleId), Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult(false));

        // When
        var result = await _controller.DeleteSale(saleId, CancellationToken.None);

        // Then
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName = "CreateSaleItem - Given a valid request, returns a CreatedResponse with status code 201")]
    public async Task CreateSaleItem_ValidRequest_ReturnsCreatedResponse()
    {
        // Given
        var saleId = Guid.NewGuid();
        var request = new CreateSaleItemRequest()
        {
            Product = DeveloperEvaluation.Domain.Common.Enums.Product.ProductQ,
            Quantity = 1,
            UnitPrice = 4
        };
        var command = new CreateSaleItemCommand { SaleId = saleId };
        var response = new CreateSaleItemResult()
        {
            Id = Guid.NewGuid(),
        };

        _mapper.Map<CreateSaleItemCommand>(request).Returns(command);
        _mediator.Send(Arg.Is<CreateSaleItemCommand>(c => c.SaleId == saleId), Arg.Any<CancellationToken>())
                 .Returns(response);

        // When
        var result = await _controller.CreateSaleItem(saleId, request, CancellationToken.None);

        // Then
        var createdResult = result as CreatedResult;
        createdResult.Should().NotBeNull();
        createdResult.StatusCode.Should().Be(201);
    }

    [Fact(DisplayName = "CreateSaleItem - Given an invalid request, returns a BadRequest with status code 400")]
    public async Task CreateSaleItem_InvalidRequest_ReturnsBadRequest()
    {
        // Given
        var saleId = Guid.NewGuid();
        var request = new CreateSaleItemRequest()
        {
            Product = (DeveloperEvaluation.Domain.Common.Enums.Product)999, 
            Quantity = 25,
            UnitPrice = 4
        };

        // When
        var result = await _controller.CreateSaleItem(saleId, request, CancellationToken.None);

        // Then
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);

        var errors = badRequestResult.Value as IEnumerable<ValidationFailure>;
        errors.Should().NotBeNull();
        errors.Should().ContainSingle(e => e.PropertyName == "Product" && e.ErrorMessage.Contains("has a range of values which does not include"));
        errors.Should().ContainSingle(e => e.PropertyName == "Quantity" && e.ErrorMessage.Contains("must be less than '20'"));
    }

    [Fact(DisplayName = "UpdateSaleItem - Given a valid request, returns an OkResponse with status code 200")]
    public async Task UpdateSaleItem_ValidRequest_ReturnsOkResponse()
    {
        // Given
        var itemId = Guid.NewGuid();
        var request = new UpdateSaleItemRequest()
        {
            Product = DeveloperEvaluation.Domain.Common.Enums.Product.ProductQ,
            IsCancelled = false,
            Quantity = 1,
            UnitPrice = 4
        };
        var command = new UpdateSaleItemCommand()
        { 
            Id = itemId,
            Product = DeveloperEvaluation.Domain.Common.Enums.Product.ProductQ,
            IsCancelled = false,
            Quantity = 1,
            UnitPrice = 4
        };
        var response = new UpdateSaleItemResult()
        {
            Id = itemId,
            Product = DeveloperEvaluation.Domain.Common.Enums.Product.ProductQ,
            IsCancelled = false,
            Quantity = 1,
            UnitPrice = 4
        };

        _mapper.Map<UpdateSaleItemCommand>(request).Returns(command);
        _mediator.Send(Arg.Is<UpdateSaleItemCommand>(c => c.Id == itemId), Arg.Any<CancellationToken>())
                 .Returns(response);

        // When
        var result = await _controller.UpdateSaleItem(itemId, request, CancellationToken.None);

        // Then
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "UpdateSaleItem - Given an invalid request (Quantity > 20), returns BadRequestResponse with status code 400")]
    public async Task UpdateSaleItem_InvalidRequest_ReturnsBadRequest()
    {
        // Given
        var itemId = Guid.NewGuid();
        var request = new UpdateSaleItemRequest()
        {
            Product = DeveloperEvaluation.Domain.Common.Enums.Product.ProductQ,
            IsCancelled = false,
            Quantity = 25,
            UnitPrice = 4
        };

        // When
        var result = await _controller.UpdateSaleItem(itemId, request, CancellationToken.None);

        // Then
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
        var validationErrors = badRequestResult.Value as IEnumerable<ValidationFailure>;
        validationErrors.Should().ContainSingle(error => error.PropertyName == "Quantity" && error.ErrorMessage.Contains("must be less than '20"));
    }

    [Fact(DisplayName = "DeleteSaleItem - Given a valid request, returns a NoContentResponse with status code 204")]
    public async Task DeleteSaleItem_ValidRequest_ReturnsNoContentResponse()
    {
        // Given
        var itemId = Guid.NewGuid();
        var command = new DeleteSaleItemCommand { Id = itemId };

        _mediator.Send(Arg.Is<DeleteSaleItemCommand>(c => c.Id == itemId), Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult(true));

        // When
        var result = await _controller.DeleteSaleItem(itemId, CancellationToken.None);

        // Then
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
    }

    [Fact(DisplayName = "DeleteSaleItem - Given item not found, returns a BadRequestResponse with status code 400")]
    public async Task DeleteSaleItem_ItemNotFound_ReturnsBadRequestResponse()
    {
        // Given
        var itemId = Guid.NewGuid();
        var command = new DeleteSaleItemCommand { Id = itemId };

        _mediator.Send(Arg.Is<DeleteSaleItemCommand>(c => c.Id == itemId), Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult(false));

        // When
        var result = await _controller.DeleteSaleItem(itemId, CancellationToken.None);

        // Then
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);

        var apiResponse = badRequestResult.Value as ApiResponse;
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.Message.Should().Be("Failed to delete sale item, sale item not found");
    }
}
