namespace Shared.DataTransferObjects
{
    public record EmployeeDto
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }        
        public string? MobileNo { get; init; }
        public string? BloodGroup { get; init; }
    }
}
