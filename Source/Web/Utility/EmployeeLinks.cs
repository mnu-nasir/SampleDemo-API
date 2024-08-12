using Contracts;
using Entities.Entities;
using Entities.LinkModels;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace Web.Utility
{
    public class EmployeeLinks : IEmployeeLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<EmployeeDto> _dataShaper;

        public Dictionary<string, MediaTypeHeaderValue> AcceptHeader { get; set; } = new Dictionary<string, MediaTypeHeaderValue>();

        public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto, string? fields, Guid companyId, HttpContext httpContext)
        {
            var shapedEmployees = ShapeData(employeesDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedEmployees(employeesDto, fields, companyId, httpContext, shapedEmployees);

            return ReturnShapedEmployees(shapedEmployees);
        }

        private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeesDto, string fields)
        {
            return _dataShaper.ShapeData(employeesDto, fields)
                .Select(e => e.Entity)
                .ToList();
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private LinkResponse ReturnShapedEmployees(List<Entity> shapedEmployees)
        {
            return new LinkResponse { ShapedEntities = shapedEmployees };
        }

        private LinkResponse ReturnLinkdedEmployees(IEnumerable<EmployeeDto> employeesDto,
            string fields, Guid companyId, HttpContext httpContext, List<Entity> shapedEmployees)
        {
            var employeeDtoList = employeesDto.ToList();

            for (var index = 0; index < employeeDtoList.Count(); index++)
            {
                var employeeLinks = CreateLinksForEmployee(httpContext, companyId, employeeDtoList[index].Id, fields);
                shapedEmployees[index].Add("Links", employeeLinks);
            }

            var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
            var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
        }

        private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid tenantId, Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeesForTenant", values: new { tenantId, fields }),"self","GET"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeeForTenant", values: new { id }),"employee_details_by_Id","GET"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteEmployeeForTenant", values: new { tenantId, id }),"delete_employee","DELETE"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateEmployee", values: new { tenantId, id }),"update_employee","PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext,
            LinkCollectionWrapper<Entity> employeesWrapper)
        {
            employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetHATEOASEmployeesForTenant", values: new { }),
                    "hateoas",
                    "GET"));

            return employeesWrapper;
        }
    }
}
