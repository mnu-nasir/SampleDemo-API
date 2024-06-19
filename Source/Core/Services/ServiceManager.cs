using Contracts;
using Service.Contracts;

namespace Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ITenantService> _tenantService;
        private readonly Lazy<IEmployeeService> _employeeService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager looger)
        {
            _tenantService = new Lazy<ITenantService>(() => new TenantService(repositoryManager, looger));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, looger));
        }

        public ITenantService TenantService => _tenantService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;
    }
}
