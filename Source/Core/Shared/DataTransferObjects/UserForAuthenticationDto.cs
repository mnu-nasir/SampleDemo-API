using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "User name or email is required")]
        public string? UserNameOrEmail { get; init; }
        
        [Required(ErrorMessage = "Password name is required")]
        public string? Password { get; init; }
    }
}
