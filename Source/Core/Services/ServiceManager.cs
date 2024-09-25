using Contracts;
using Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ITenantService> _tenantService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<IAuthenticationService> _authenticationService;

    public ServiceManager(
        IRepositoryManager repositoryManager,
        ILoggerManager logger,
        IDataShaper<TenantDto> tenantDataShaper,
        IDataShaper<EmployeeDto> employeeDataShaper,
        ITenantLinks tenantLinks,
        IEmployeeLinks employeeLinks,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _tenantService = new Lazy<ITenantService>(() => new TenantService(repositoryManager, logger, tenantDataShaper, tenantLinks));
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, logger, employeeDataShaper, employeeLinks));
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, userManager, configuration));
    }

    public ITenantService TenantService => _tenantService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
