
# PagedList Library for IQueryable with Filtering, Sorting and Pagination

A powerful .NET library that extends `IQueryable` to support efficient filtering, sorting, and pagination. Simplify the management of large datasets with easy-to-use methods.

## Features

### Pagination
Efficiently paginate any `IQueryable` source with customizable page size and index.
- Supports both synchronous and asynchronous pagination
- Automatically calculates total items and total pages
- Handles edge cases like out-of-range page indices

### Dynamic Filtering
Apply flexible filters to `IQueryable` sources using various conditions.
- Supports multiple comparison operators (==, !=, >, <, >=, <=, Contains, StartsWith, EndsWith)
- Allows for complex nested conditions
- Dynamically builds LINQ expressions based on filter criteria

### Sorting
Order `IQueryable` sources by specified properties in ascending or descending order.

- Handles nested property sorting (e.g., "User.Name")

### Async Support
Asynchronous methods for improved performance in web applications.
- Leverages `Task`-based asynchronous programming
- Optimized for scalability in high-concurrency scenarios

### Entity Agnostic
Works with any entity type, providing maximum flexibility.
- No need for special attributes or interfaces on your models
- Compatible with custom types and complex object graphs

## Installation

Install via NuGet Package Manager Console:

```shell
Install-Package API.PagedList
```

Or via .NET CLI:

```shell
dotnet add package API.PagedList
```

## Quick Start

```csharp
using API.PagedList;
using API.PagedList.Model;

// Create a filter
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

// Apply filter and paginate
var result = await myQueryableSource.ToPagedListAsync(filter);
```

## Usage Examples

### Basic Pagination

```csharp
var pagedList = myQueryableSource.ToPagedList(pageSize: 10, pageIndex: 1);
```

### Async Pagination

```csharp
var pagedList = await myQueryableSource.ToPagedListAsync(pageSize: 10, pageIndex: 1);
```

### Complex Filtering and Sorting

```csharp
var filter = new FilterVM
{
    PageSize = 10,
    PageIndex = 1,
    Conditions = new List<WhereVM>
    {
        new WhereVM { Name = "Property1", Comparison = "==", Value = "Value1" },
        new WhereVM { Name = "Property2", Comparison = ">", Value = "Value2" }
    },
    OrderBy = new OrderVM { Name = "Property1", Ascending = true }
};

var result = await myQueryableSource.ToPagedListAsync(filter);
```

## API Controller Example
![image](https://drive.google.com/uc?export=view&id=1YbkGPhVsnZlIcVVFcJemyX5p55yori0p)
```csharp
[HttpPost("GetWeatherForecast")]
public async Task<IActionResult> GetWeatherForecast([FromBody] FilterVM filter)
{
    IQueryable<WeatherForecast> models = // ... your data source
    var pagedList = await models.ToPagedListAsync(filter);
    return Ok(pagedList);
}
```

![image](https://drive.google.com/uc?export=view&id=1E-2xCbVXs4BmokoLFW3wqumRMOJXu6wX)

## Performance Considerations

- The library uses expression trees to build efficient LINQ queries
- Filtering and sorting are performed at the database level when possible
- Pagination reduces memory usage by fetching only the required records

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
