using Entities.Entities;
using Persistence.Repositories.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Persistence.Repositories.Extensions;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAage, uint maxAze)
    {
        var filterResult = employees.Where(e => (e.Age >= minAage && e.Age <= maxAze));
        return filterResult;
    }

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return employees;

        return employees.Where(e => e.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                             || e.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return employees.OrderBy(e => e.FirstName);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Tenant>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return employees.OrderBy(e => e.FirstName);

        return employees.OrderBy(orderQuery);
    }
}
