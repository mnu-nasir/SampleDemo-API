namespace Shared.RequestFeatures
{
    public sealed class EmployeeParameters : RequestParameters
    {
        public EmployeeParameters()
        {
            OrderBy = "FirstName";
        }

        public uint MinAge { get; set; }
        public uint MaxAge { get; set; } = int.MaxValue;

        public bool ValidAgeRange => MaxAge > MinAge;
        public string? SearchTerm { get; set; }
    }
}
