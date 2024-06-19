namespace Contracts
{
    public interface ITenantResolver
    {
        Guid GetCurrentTenant();        
    }
}
