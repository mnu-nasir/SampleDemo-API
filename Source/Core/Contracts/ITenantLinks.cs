using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DataTransferObjects;

namespace Contracts;

public interface ITenantLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<TenantDto> tenantDto, string? fields, HttpContext httpContext);
}
