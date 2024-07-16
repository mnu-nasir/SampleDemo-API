namespace Entities.Exceptions
{
    public sealed class TenantCollectionBadRequestException : BadRequestException
    {
        public TenantCollectionBadRequestException()
            :base("Tenant collection sent from the client is null.")
        {            
        }
    }
}
