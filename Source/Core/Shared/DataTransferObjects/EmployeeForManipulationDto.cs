using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public abstract record EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Title is required")]
        public required string FirstName { get; init; }

        [Required(ErrorMessage = "Title is required")]
        public required string LastName { get; init; }

        [Required(ErrorMessage = "Title is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public required string Email { get; init; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        public string? MobileNo { get; init; }
        public string? BloodGroup { get; init; }
    }
}
