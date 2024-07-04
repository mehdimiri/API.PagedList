using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.PagedList;

public static class QueryableExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, string property, object value)
    {
        return queryable.Filter(property, string.Empty, value);
    }

    public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, string property, string comparison, object value)
    {
        if (string.IsNullOrWhiteSpace(property) || value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return queryable;

        ParameterExpression parameter = Expression.Parameter(typeof(T));
        Expression left = Create(property, parameter);

        try
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(property);
            if (propertyInfo is null)
                return queryable;

            Type type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            value = Change(value, type);

            ConstantExpression right = Expression.Constant(value, left.Type);
            Expression body = Create(left, comparison, right);
            var expression = Expression.Lambda<Func<T, bool>>(body, parameter);
            return queryable.Where(expression);
        }
        catch
        {
            return Enumerable.Empty<T>().AsQueryable();
        }
    }

    public static IQueryable<T> Order<T>(this IQueryable<T> queryable, string property, bool ascending)
    {
        if (queryable is null || string.IsNullOrWhiteSpace(property))
            return queryable;

        ParameterExpression parameter = Expression.Parameter(typeof(T));
        Expression body = Create(property, parameter);
        var expression = (dynamic)Expression.Lambda(body, parameter);
        return ascending ? Queryable.OrderBy(queryable, expression) : Queryable.OrderByDescending(queryable, expression);
    }

    public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int index, int size)
    {
        if (queryable is null || index <= 0 || size <= 0)
            return queryable;
        return queryable.Skip((index - 1) * size).Take(size);
    }

    #region Private

    private static object Change(object value, Type type)
    {
        if (type.IsEnum)
        {
            ArgumentNullException.ThrowIfNull(value);
            string stringValue = value.ToString();
            if (string.IsNullOrEmpty(stringValue))
                return Activator.CreateInstance(type);
            return Enum.Parse(type, stringValue, ignoreCase: true);
        }
        else if (value is JsonElement jsonElement)
        {
            return ConvertJsonElement(jsonElement, type);
        }
        else if (value is IConvertible)
        {
            return Convert.ChangeType(value, type);
        }
        else
        {
            throw new InvalidCastException($"Cannot convert value of type {value.GetType()} to {type}.");
        }
    }

    private static object ConvertJsonElement(JsonElement jsonElement, Type type)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.String:
                if (type == typeof(DateOnly) && DateOnly.TryParse(jsonElement.ToString(), out DateOnly dateOnly))
                {
                    return dateOnly;
                }
                else if (type == typeof(TimeOnly) && TimeOnly.TryParse(jsonElement.ToString(), out TimeOnly timeOnly))
                {
                    return timeOnly;
                }
                else if (type == typeof(DateTimeOffset) && DateTimeOffset.TryParse(jsonElement.ToString(), out DateTimeOffset dateTimeOffset))
                {
                    return dateTimeOffset;
                }
                else if (type == typeof(DateTime) && DateTime.TryParse(jsonElement.ToString(), out DateTime dateTime))
                {
                    return dateTime;
                }
                else if (type == typeof(Guid) && Guid.TryParse(jsonElement.ToString(), out Guid guid))
                {
                    return guid;
                }
                return jsonElement.GetString();
            case JsonValueKind.Number:
                if (type == typeof(int))
                    return jsonElement.GetInt32();
                if (type == typeof(long))
                    return jsonElement.GetInt64();
                if (type == typeof(double))
                    return jsonElement.GetDouble();
                if (type == typeof(decimal))
                    return jsonElement.GetDecimal();
                break;
            case JsonValueKind.True:
            case JsonValueKind.False:
                if (type == typeof(bool))
                    return jsonElement.GetBoolean();
                break;
            case JsonValueKind.Null:
                return null;
        }
        throw new InvalidCastException($"Cannot convert JsonElement of kind {jsonElement.ValueKind} to {type}.");
    }

    private static Expression Create(string property, Expression parameter)
    {
        return property.Split('.').Aggregate(parameter, Expression.Property);
    }

    private static Expression Create(Expression left, string comparison, Expression right)
    {
        if (!string.IsNullOrEmpty(comparison) && left.Type == typeof(string))
        {
            if (comparison.Equals("contains", StringComparison.CurrentCultureIgnoreCase))
                return Expression.Call(left, nameof(string.Contains), Type.EmptyTypes, right);
            else if (comparison.Equals("startswith", StringComparison.CurrentCultureIgnoreCase))
                return Expression.Call(left, nameof(string.StartsWith), Type.EmptyTypes, right);
            else if (comparison.Equals("endswith", StringComparison.CurrentCultureIgnoreCase))
                return Expression.Call(left, nameof(string.EndsWith), Type.EmptyTypes, right);
        }

        var type = comparison switch
        {
            "<" => ExpressionType.LessThan,
            "<=" => ExpressionType.LessThanOrEqual,
            ">" => ExpressionType.GreaterThan,
            ">=" => ExpressionType.GreaterThanOrEqual,
            "!=" => ExpressionType.NotEqual,
            _ => ExpressionType.Equal
        };
        return Expression.MakeBinary(type, left, right);
    }

    #endregion
}
