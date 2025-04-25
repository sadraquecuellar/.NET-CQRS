using Ambev.DeveloperEvaluation.Common.Helpers;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
///     Implementation of ISalesRepository using EF Core
/// </summary>
public class SalesRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    ///     Initializes a new instance of SalesRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SalesRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sale.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    ///     Returns a sale by Id
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sale.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    ///     Retrieves all sales
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of all sales</returns>
    public async Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sale.Include(s => s.Items).ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     Returns a list of sales with optional filtering, pagination, and sorting parameters.
    /// </summary>
    /// <param name="saleNumber">Optional sale number to filter results</param>
    /// <param name="isCanceled">Optional flag to filter by canceled status</param>
    /// <param name="branch">Optional branch to filter results</param>
    /// <param name="customer">Optional customer to filter results</param>
    /// <param name="saleDateFrom">Optional starting date to filter sales from</param>
    /// <param name="saleDateTo">Optional end date to filter sales until</param>
    /// <param name="page">The page number for pagination (default is 0 - returns all results)</param>
    /// <param name="pageSize">The number of items per page (default is 0 - returns all results)</param>
    /// <param name="sortBy">Optional field name to sort results by. If null, no sorting is applied.</param>
    /// <param name="isDescending">Indicates whether sorting should be descending (true) or ascending (false). Default is false.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the async operation if needed.</param>
    /// <returns>
    ///     A paginated list of sales that match the given filtering parameters, including associated items.
    /// </returns>
    public async Task<PagedList<Sale>> GetListAsync(string? saleNumber = null, bool? isCanceled = null,
        Branch? branch = null, Customer? customer = null, DateTime? saleDateFrom = null, DateTime? saleDateTo = null,
        int page = 0, int pageSize = 0, string? sortBy = null, bool isDescending = false, CancellationToken cancellationToken = default)
    {
        var query = _context.Sale
            .Where(x =>
                (saleNumber == null || saleNumber == x.SaleNumber)
                && (isCanceled == null || isCanceled == x.IsCancelled)
                && (branch == null || branch == x.Branch)
                && (customer == null || customer == x.Customer)
                && (saleDateFrom == null || x.Date >= saleDateFrom)
                && (saleDateTo == null || x.Date <= saleDateTo)
            ).Include(s => s.Items);
        return await PagedList<Sale>.CreatePaginationAsync(query, page, pageSize, sortBy, isDescending, cancellationToken);
    }

    // <summary>
    /// Returns a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">Sale number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sale.FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    ///     Updates an existing sale
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sale.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     Updates an existing sale item
    /// </summary>
    /// <param name="saleItem">The sale item to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task UpdateItemAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
    {
        _context.SaleItem.Update(saleItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     Deletes a sale
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.Sale.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    ///     Deletes a sale item
    /// </summary>
    /// <param name="id">The Id of the sale item</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale item was deleted, false if not found</returns>
    public async Task<bool> DeleteItemAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetItemByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.SaleItem.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    ///     Retrieves a sale item
    /// </summary>
    /// <param name="id">The Id of the sale item</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale item if found, null otherwise</returns>
    public async Task<SaleItem?> GetItemByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SaleItem.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}