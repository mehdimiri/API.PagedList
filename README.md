# PagedList Library for IQueryable with Filtering, Sorting and Pagination

Welcome to the PagedList library! This library extends `IQueryable` to support efficient Filtering, Sorting and Pagination. It's designed to simplify the process of handling large datasets by providing easy-to-use methods for Filtering, Sorting and Paging.

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

![image](https://drive.google.com/uc?export=view&id=1YbkGPhVsnZlIcVVFcJemyX5p55yori0p)

```csharp
 [ApiController]
 [Route("[controller]")]
 public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
 {
     private static readonly string[] Summaries =
     [
         "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
     ];

     private readonly ILogger<WeatherForecastController> _logger = logger;

     [HttpPost("GetWeatherForecast")]
     public dynamic Get(FilterVM filter)
     {
         IQueryable<WeatherForecast> models = Enumerable.Range(1, 10).Select(index => new WeatherForecast
         {
             Date = DateTime.Now.AddDays(index),
             TemperatureC = Random.Shared.Next(-20, 55),
             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
         }).AsQueryable();
        return models.ToPagedList(filter);
     }
 }
```
![image](https://drive.google.com/uc?export=view&id=1E-2xCbVXs4BmokoLFW3wqumRMOJXu6wX)

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

### Filtering & Sorting

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


## License

This project is licensed under the MIT License.

---

