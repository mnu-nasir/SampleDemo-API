namespace Service.Contracts;

public interface IServiceManager
{
    ITenantService TenantService { get; }
    IEmployeeService EmployeeService { get; }
    IAuthenticationService AuthenticationService { get; }
}
