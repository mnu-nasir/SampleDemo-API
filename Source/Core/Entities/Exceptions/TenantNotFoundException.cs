namespace Entities.Exceptions;

public sealed class TenantNotFoundException : NotFoundException
{
    public TenantNotFoundException(Guid tenantId)
        : base($"The tenant with Id {tenantId} does not exist into the database.")
    {

    }
}
