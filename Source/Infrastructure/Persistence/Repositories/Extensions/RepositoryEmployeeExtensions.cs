using Entities.Entities;

namespace Persistence.Repositories.Extensions
{
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

            return employees.Where(e =>
                                e.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                        || e.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }
}
