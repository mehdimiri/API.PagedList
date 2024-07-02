# PagedList Library for IQueryable with Filtering and Pagination

Welcome to the PagedList library! This library extends `IQueryable` to support efficient pagination and filtering. It's designed to simplify the process of handling large datasets by providing easy-to-use methods for paging and filtering.

## Features

- **Pagination**: Paginate any `IQueryable` source with customizable page size and index, improving performance by fetching only the needed records.
- **Filtering**: Apply dynamic filters on `IQueryable` sources using various conditions, allowing for flexible and efficient querying.
- **Sorting**: Order `IQueryable` sources by specified properties in ascending or descending order, making it easy to sort results based on user preferences.

## Installation

To install the library, you can use the NuGet Package Manager Console:

```shell
pm> Install-Package API.PagedList
```

## Usage

### Pagination

You can easily paginate your `IQueryable` source using the `ToPagedList` extension methods.

```csharp
using API.PagedList;

var pagedList = myQueryableSource.ToPagedList(pageSize: 10, pageIndex: 1);
```

For asynchronous operations:

```csharp
using API.PagedList;
using System.Threading.Tasks;

var pagedList = await myQueryableSource.ToPagedListAsync(pageSize: 10, pageIndex: 1);
```

### Filtering

The library also supports dynamic filtering based on conditions specified in a `FilterVM` object.

```csharp
using API.PagedList;
using API.PagedList.Model;
using System.Collections.Generic;

var filter = new FilterVM
{
    PageSize = 10,
    PageIndex = 1,
    Conditions = new List<WhereVM>
    {
        new WhereVM { Name = "PropertyName", Comparison = "==", Value = "Value" }
    },
    OrderBy = new OrderVM { Name = "PropertyName", Ascending = true }
};

var filteredPagedList = myQueryableSource.ToPagedList(filter);
```

For asynchronous filtering and pagination:

```csharp
using API.PagedList;
using API.PagedList.Model;
using System.Threading.Tasks;

var filteredPagedList = await myQueryableSource.ToPagedListAsync(filter);
```

## Example

Here's an example of how you might use the PagedList library in a DbContext and a service class:

```csharp
using API.PagedList;
using API.PagedList.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class MyDbContext : DbContext
{
    public DbSet<MyEntity> MyEntities { get; set; }
}

public class MyService
{
    private readonly MyDbContext _context;

    public MyService(MyDbContext context)
    {
        _context = context;
    }

    // Get a paged list of entities
    public async Task<PagedListResult<MyEntity>> GetPagedEntities(int pageIndex, int pageSize)
    {
        var query = _context.MyEntities.AsQueryable();
        return await query.ToPagedListAsync(pageSize, pageIndex);
    }

    // Get a filtered and paged list of entities
    public async Task<PagedListResult<MyEntity>> GetFilteredPagedEntities(FilterVM filter)
    {
        var query = _context.MyEntities.AsQueryable();
        return await query.ToPagedListAsync(filter);
    }
}
```

## License

This project is licensed under the MIT License.

---

