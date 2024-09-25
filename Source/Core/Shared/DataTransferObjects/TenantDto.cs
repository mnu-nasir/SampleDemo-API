namespace Shared.DataTransferObjects;

public record TenantDto
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Address { get; init; }
}
