using Contracts;
using Microsoft.AspNetCore.Http;

namespace Persistence.Resolvers;

public sealed class UserAccountResolver(IHttpContextAccessor contextAccessor) : IUserAccountResolver
{
    public Guid GetCurrentUserAccount()
    {
        var userClaim = (contextAccessor?.HttpContext?
            .User.Claims.FirstOrDefault(e => e.Type == "AccountId"));

        if (userClaim is null)
        {
            throw new UnauthorizedAccessException("Authentication failed");
        }

        string? claimUserId = userClaim.Value?.ToString();
        bool result = Guid.TryParse(claimUserId, out Guid accountId);

        if (!result)
        {
            throw new UnauthorizedAccessException("Authentication failed");
        }

        return accountId;
    }
}
