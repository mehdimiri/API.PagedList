using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.PagedList.Model;

public class PagedListResult<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<T> Items { get; set; }
    public bool HasNextPage
    {
        get { return PageIndex < TotalPages; }
    }

    public bool HasPreviousPage
    {
        get { return PageIndex > 1; }
    }
}