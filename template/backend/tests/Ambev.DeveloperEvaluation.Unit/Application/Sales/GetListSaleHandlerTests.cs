using Ambev.DeveloperEvaluation.Application.Sales.GetListSale;
using Ambev.DeveloperEvaluation.Common.Helpers;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;
/// <summary>
///     Contains unit tests for the <see cref="GetListSaleHandlerTests" /> class.
/// </summary>
public class GetListSaleHandlerTests
{
    private readonly GetListSaleHandler _handler;
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;

    public GetListSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetListSaleHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "When getting sales list without filters Then returns all sales with pagination")]
    public async Task Handle_WithoutFilters_ShouldReturnAllSalesWithPagination()
    {
        // Given
        var command = new GetListSaleCommand { Page = 1, PageSize = 5 };
        var salesList = GetListSaleHandlerTestData.GenerateMockSales();
        var pagedSales = new PagedList<Sale>
        {
            Items = [.. salesList.Take(command.PageSize)],
            Page = command.Page,
            PageSize = command.PageSize,
            TotalCount = salesList.Count
        };

        _saleRepository.GetListAsync(Arg.Any<string>(), Arg.Any<bool?>(), Arg.Any<Branch?>(), Arg.Any<Customer?>(),
            Arg.Any<DateTime?>(), Arg.Any<DateTime?>(), command.Page, command.PageSize, Arg.Any<string>(),
            Arg.Any<bool>(), Arg.Any<CancellationToken>()).Returns(pagedSales);

        var expectedResult = salesList.Select(sale => new GetListSaleResult
        {
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.Date,
            Customer = sale.Customer,
            Branch = sale.Branch
        }).Take(command.PageSize).ToList();

        _mapper.Map<List<GetListSaleResult>>(pagedSales.Items).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Items.Should().NotBeNullOrEmpty();
        result.Items.Should().HaveCount(command.PageSize);
        result.Page.Should().Be(command.Page);
        result.PageSize.Should().Be(command.PageSize);
        result.TotalCount.Should().Be(salesList.Count);
    }

    [Fact(DisplayName = "When getting sales list with pagination Then returns correct page")]
    public async Task Handle_WithPagination_ShouldReturnCorrectPage()
    {
        // Given
        var command = new GetListSaleCommand { Page = 2, PageSize = 3 };

        const int totalItems = 10;
        var salesList = GetListSaleHandlerTestData.GenerateMockSales(totalItems);

        var pagedSales = new PagedList<Sale>
        {
            Items = salesList.Skip((command.Page - 1) * command.PageSize).Take(command.PageSize).ToList(),
            Page = command.Page,
            PageSize = command.PageSize,
            TotalCount = totalItems
        };

        _saleRepository.GetListAsync(
                Arg.Is<string?>(s => s == command.SaleNumber),
                Arg.Is<bool?>(b => b == command.IsCanceled),
                Arg.Is<Branch?>(b => b == command.Branch),
                Arg.Is<Customer?>(c => c == command.Customer),
                Arg.Is<DateTime?>(d => d == command.SaleDateFrom),
                Arg.Is<DateTime?>(d => d == command.SaleDateTo),
                Arg.Is<int>(p => p == command.Page),
                Arg.Is<int>(ps => ps == command.PageSize),
                Arg.Is<string?>(s => s == command.SortBy),
                Arg.Is<bool>(b => b == command.IsDesc),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(pagedSales));

        var expectedResult = pagedSales.Items.Select(sale => new GetListSaleResult
        {
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.Date,
            Customer = sale.Customer,
            Branch = sale.Branch,
        }).ToList();

        _mapper.Map<List<GetListSaleResult>>(pagedSales.Items).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Items.Should().NotBeNullOrEmpty();
        result.Items.Should().HaveCount(command.PageSize);
        result.Page.Should().Be(command.Page);
        result.PageSize.Should().Be(command.PageSize);
        result.TotalCount.Should().Be(salesList.Count);
    }

}