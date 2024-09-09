namespace Entities.Entities
{
    public sealed class Tenant : BaseEntity
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
        public bool? IsPrimary { get; set; }
        public ICollection<Employee> Employees { get; set; } = [];
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = [];
    }
}
