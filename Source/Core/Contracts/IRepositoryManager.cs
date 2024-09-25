namespace Contracts;

public interface IRepositoryManager
{
    ITenantRepository Tenant { get; }
    IEmployeeRepository Employee { get; }
    Task SaveChangesAsync();
}
