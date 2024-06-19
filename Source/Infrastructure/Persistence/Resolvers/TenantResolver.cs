using Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Resolvers
{
    public sealed class TenantResolver(IHttpContextAccessor contextAccessor) : ITenantResolver
    {
        public Guid GetCurrentTenant()
        {
            var tenantClaim = (contextAccessor?.HttpContext?
                .User.Claims.FirstOrDefault(e => e.Type == "TenantId"));

            if (tenantClaim is null)
            {
                throw new UnauthorizedAccessException("Authentication failed");
            }

            string? claimTenantId = tenantClaim.Value?.ToString();
            bool result = Guid.TryParse(claimTenantId, out Guid tenant);
            if (!result)
            {
                throw new UnauthorizedAccessException("Authentication failed");
            }

            return tenant;
        }
    }
}
