using System.ComponentModel.DataAnnotations;

namespace Entities.Entities;

public abstract class BaseEntity<T> : BaseEntity
{
    public T? Id { get; set; }
    public required Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}

public abstract class BaseEntity
{
    [MaxLength(50)]
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }

    [MaxLength(50)]
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    [MaxLength(50)]
    public string? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}
