using Contracts;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ITenantService> _tenantService;
        private readonly Lazy<IEmployeeService> _employeeService;

        public ServiceManager(
            IRepositoryManager repositoryManager,
            ILoggerManager looger,
            IDataShaper<TenantDto> tenantDataShaper,
            IDataShaper<EmployeeDto> employeeDataShaper,
            ITenantLinks tenantLinks,
            IEmployeeLinks employeeLinks)
        {
            _tenantService = new Lazy<ITenantService>(() => new TenantService(repositoryManager, looger, tenantDataShaper, tenantLinks));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, looger, employeeDataShaper, employeeLinks));
        }

        public ITenantService TenantService => _tenantService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;
    }
}
