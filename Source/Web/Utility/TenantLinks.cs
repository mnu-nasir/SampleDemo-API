using Contracts;
using Entities.Entities;
using Entities.LinkModels;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace Web.Utility
{
    public class TenantLinks : ITenantLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<TenantDto> _dataShaper;

        public Dictionary<string, MediaTypeHeaderValue> AcceptHeader { get; set; } =
            new Dictionary<string, MediaTypeHeaderValue>();

        public TenantLinks(LinkGenerator linkGenerator, IDataShaper<TenantDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private List<Entity> ShapeData(IEnumerable<TenantDto> tenantDto, string? fields)
        {
            return _dataShaper.ShapeData(tenantDto, fields)
                .Select(e => e.Entity)
                .ToList();
        }

        private List<Link> CreateLinksForTenant(HttpContext httpContext, Guid id, string? fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany", values: new { id, fields }),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteEmployeeForCompany", values: new { id }),
                "delete_employee",
                "DELETE"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateEmployeeForCompany", values: new { id }),
                "update_employee",
                "PUT"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateEmployeeForCompany", values: new { id }),
                "partially_update_employee",
                "PATCH")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForTenants(HttpContext httpContext,
            LinkCollectionWrapper<Entity> tenantsWrapper)
        {
            tenantsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeesForCompany", values: new { }),
                    "self",
                    "GET"));

            return tenantsWrapper;
        }

        private LinkResponse ReturnLinkdedTenants(IEnumerable<TenantDto> tenantsDto,
            string? fields, HttpContext httpContext, List<Entity> shapedTenants)
        {
            var tenantDtoList = tenantsDto.ToList();

            for (var index = 0; index < tenantDtoList.Count(); index++)
            {
                var tenantLinks = CreateLinksForTenant(httpContext, tenantDtoList[index].Id, fields);
                shapedTenants[index].Add("Links", tenantLinks);
            }

            var tenantCollection = new LinkCollectionWrapper<Entity>(shapedTenants);
            var linkedEmployees = CreateLinksForTenants(httpContext, tenantCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
        }

        private LinkResponse ReturnShapedTenants(List<Entity> shapedTenants)
        {
            return new LinkResponse { ShapedEntities = shapedTenants };
        }

        public LinkResponse TryGenerateLinks(IEnumerable<TenantDto> tenantDto, string? fields, HttpContext httpContext)
        {
            var shapedTenants = ShapeData(tenantDto, fields);

            if (ShouldGenerateLinks(httpContext))
            {
                return ReturnLinkdedTenants(tenantDto, fields, httpContext, shapedTenants);
            }

            return ReturnShapedTenants(shapedTenants);
        }
    }
}