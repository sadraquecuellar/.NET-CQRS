using Ambev.DeveloperEvaluation.Common.Helpers;
using Ambev.DeveloperEvaluation.Domain.Common.Enums;
using Ambev.DeveloperEvaluation.Domain.Sales.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
/// <summary>
///     Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    ///     Creates a new sale in the repository
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a new sale item in the repository
    /// </summary>
    /// <param name="saleItem">The sale item to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale item</returns>
    Task<SaleItem> CreateItemAsync(SaleItem saleItem, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns a sale by Id
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns all sales from the repository
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of all sales</returns>
    Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing sale
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a sale
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    // <summary>
    /// Returns a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">Sale number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);

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
    Task<PagedList<Sale>> GetListAsync(string? saleNumber = null, bool? isCanceled = null,
        Branch? branch = null, Customer? customer = null, DateTime? saleDateFrom = null, DateTime? saleDateTo = null,
        int page = 0, int pageSize = 0, string? sortBy = null, 
        bool isDescending = false, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a sale item
    /// </summary>
    /// <param name="id">The Id of the sale item</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale item was deleted, false if not found</returns>
    Task<bool> DeleteItemAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a sale item
    /// </summary>
    /// <param name="id">The Id of the sale item</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale item if found, null otherwise</returns>
    Task<SaleItem?> GetItemByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing sale item
    /// </summary>
    /// <param name="saleItem">The sale item to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UpdateItemAsync(SaleItem saleItem, CancellationToken cancellationToken = default);
}
