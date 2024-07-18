namespace Shared.RequestFeatures
{
    public sealed class TenantParameters : RequestParameters
    {
        public bool? IsActive { get; set; }
        public string? SearchTerm { get; set; }
    }
}
