using API.PagedList.Model;
using JcoCommon.Extensions;
using Microsoft.EntityFrameworkCore;

namespace API.PagedList;

public static class PagedListExtensions
{
    public static PagedListResult<T> ToPagedList<T>(this IQueryable<T> query, int pageSize, int pageIndex)
    {
        int totalItemCount = query is null ? 0 : query.Count();
        return new PagedListResult<T>
        {
            TotalCount = totalItemCount,
            PageSize = pageSize,
            PageIndex = pageIndex,
            TotalPages = totalItemCount > 0 ? (int)Math.Ceiling(totalItemCount / (double)pageSize) : 0,
            Items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
        };
    }
    public static async Task<PagedListResult<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageSize, int pageIndex)
    {
        int totalItemCount = query is null ? 0 : query.Count();
        return new PagedListResult<T>
        {
            TotalCount = totalItemCount,
            PageSize = pageSize,
            PageIndex = pageIndex,
            TotalPages = totalItemCount > 0 ? (int)Math.Ceiling(totalItemCount / (double)pageSize) : 0,
            Items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(),
        };
    }
    public static PagedListResult<T> ToPagedList<T>(this IQueryable<T> query, FilterVM filter)
    {
       
        if (filter.Conditions.Count != 0)
        {
            foreach (var item in filter.Conditions)
            {
                query = query.Filter(item.Name, item.Comparison, item.Value);
            }
        }
        int totalItemCount = query is null ? 0 : query.Count();
        if (filter.OrderBy is not null)
        {
            query = query.Order(filter.OrderBy.Name, filter.OrderBy.Ascending);
        }
        return new PagedListResult<T>
        {
            TotalCount = totalItemCount,
            PageSize = filter.PageSize,
            PageIndex = filter.PageIndex,
            TotalPages = totalItemCount > 0 ? (int)Math.Ceiling(totalItemCount / (double)filter.PageSize) : 0,
            Items = [.. query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize)],
        };
    }
    public static async Task<PagedListResult<T>> ToPagedListAsync<T>(this IQueryable<T> query, FilterVM filter)
    {

        if (filter.Conditions.Count != 0)
        {
            foreach (var item in filter.Conditions)
            {
                query = query.Filter(item.Name, item.Comparison, item.Value);
            }
        }
        int totalItemCount = query is null ? 0 : query.Count();
        if (filter.OrderBy is not null)
        {
            query = query.Order(filter.OrderBy.Name, filter.OrderBy.Ascending);
        }
        return new PagedListResult<T>
        {
            TotalCount = totalItemCount,
            PageSize = filter.PageSize,
            PageIndex = filter.PageIndex,
            TotalPages = totalItemCount > 0 ? (int)Math.Ceiling(totalItemCount / (double)filter.PageSize) : 0,
            Items = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync(),
        };
    }
}
