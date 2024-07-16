using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public abstract record TenantForManipulationDto
    {
        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; init; }

        [Required(ErrorMessage = "Address is required")]
        public required string Address { get; init; }
    }
}
