using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "First name is required")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public required string LastName { get; set; }

        public string? DisplayName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; init; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        
        [Required(ErrorMessage = "Tenant id is required")]
        public Guid TenantId { get; set; }

        [Required(ErrorMessage = "Create by is required")]
        public string? CreatedBy { get; set; }

        public ICollection<string>? Roles { get; init; }
    }
}
