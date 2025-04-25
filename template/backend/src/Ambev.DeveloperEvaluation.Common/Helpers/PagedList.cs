using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Common.Helpers;
public class PagedList<T>
{
    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;

    public static async Task<PagedList<T>> CreatePaginationAsync(
                                                    IQueryable<T> query, 
                                                    int page = 0, int pageSize = 0,
                                                    string? sortBy = null, bool isDescending = false, 
                                                    CancellationToken cancellationToken = default)
    {
        int totalCount = query.Count();

        if (!string.IsNullOrEmpty(sortBy))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = isDescending ? "OrderByDescending" : "OrderBy";
            var orderByExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(T), property.Type],
                query.Expression,
                Expression.Quote(lambda)
            );

            query = query.Provider.CreateQuery<T>(orderByExpression);
        }

        List<T> items;
        if (page != 0 && pageSize != 0)
            items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        else
            items = await query.ToListAsync(cancellationToken);

        return new()
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}