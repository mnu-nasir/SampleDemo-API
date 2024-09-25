using Microsoft.AspNetCore.Identity;

namespace Entities.Entities;

public sealed class ApplicationRole : IdentityRole<Guid>
{
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}
