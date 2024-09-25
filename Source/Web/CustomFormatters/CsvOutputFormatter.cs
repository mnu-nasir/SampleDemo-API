using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;
using System.Text;

namespace Web.CustomFormatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (typeof(TenantDto).IsAssignableFrom(type)
            || typeof(IEnumerable<TenantDto>).IsAssignableFrom(type)
            || typeof(EmployeeDto).IsAssignableFrom(type)
            || typeof(IEnumerable<EmployeeDto>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }

        return false;
    }

    public override async Task WriteResponseBodyAsync(
        OutputFormatterWriteContext context,
        Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object is IEnumerable<TenantDto> tenants)
        {
            foreach (var tenant in tenants)
            {
                FormatTenantCsv(buffer, tenant);
            }
        }
        else if (context.Object is TenantDto tenant)
        {
            FormatTenantCsv(buffer, tenant);
        }
        else if (context.Object is IEnumerable<EmployeeDto> employees)
        {
            foreach (var employee in employees)
            {
                FormatAccountCsv(buffer, employee);
            }
        }
        else if (context.Object is EmployeeDto employee)
        {
            FormatAccountCsv(buffer, employee);
        }
        else
        {
            buffer.Append("Formatter is not found for this Return Type!");
        }

        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatTenantCsv(StringBuilder buffer, TenantDto tenant)
    {
        buffer.AppendLine($"{tenant.Id},\"{tenant.Title},\"{tenant.Address}\"");
    }

    private static void FormatAccountCsv(StringBuilder buffer, EmployeeDto employee)
    {
        buffer.AppendLine($"{employee.Id},\"{employee.FirstName},\"{employee.LastName},\"{employee.Email},\"{employee.MobileNo},\"{employee.BloodGroup}\"");
    }
}
