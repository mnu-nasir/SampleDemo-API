using Entities.Enums;

namespace Entities.Entities
{
    public sealed class Employee : BaseEntity<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? MobileNo { get; set; }
        public BloodGroup? BloodGroup { get; set; }
        public uint Age { get; set; }
    }
}
