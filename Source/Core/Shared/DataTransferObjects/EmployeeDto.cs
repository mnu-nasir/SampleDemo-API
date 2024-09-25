namespace Shared.DataTransferObjects;

public record EmployeeDto
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? MobileNo { get; init; }
    public string? BloodGroup { get; init; }
}
