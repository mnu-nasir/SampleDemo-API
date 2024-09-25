using Contracts;
using Persistence.DbContexts;

namespace Persistence.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly TenantContext _tenantContext;
    private readonly ApplicationContext _applicationContext;
    private readonly AuthenticationContext _authenticationContext;

    private readonly Lazy<ITenantRepository> _tenantRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;

    public RepositoryManager(
        TenantContext tenantContext,
        ApplicationContext applicationContext,
        AuthenticationContext authenticationContext)
    {
        _tenantContext = tenantContext;
        _applicationContext = applicationContext;
        _authenticationContext = authenticationContext;

        _tenantRepository = new Lazy<ITenantRepository>(() => new TenantRepository(tenantContext));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(applicationContext));
    }

    public ITenantRepository Tenant => _tenantRepository.Value;
    public IEmployeeRepository Employee => _employeeRepository.Value;

    public async Task SaveChangesAsync()
    {
        if (_tenantContext?.ChangeTracker.HasChanges() == true)
        {
            await _tenantContext.SaveChangesAsync();
        }

        if (_applicationContext?.ChangeTracker.HasChanges() == true)
        {
            await _applicationContext.SaveChangesAsync();
        }

        if (_authenticationContext?.ChangeTracker.HasChanges() == true)
        {
            await _authenticationContext.SaveChangesAsync();
        }
    }
}
