using System.Runtime.Serialization;

namespace Entities.Enums
{
    public enum BloodGroup
    {
        [EnumMember(Value = "O+")]
        OPositive,

        [EnumMember(Value = "A+")]
        APositive,

        [EnumMember(Value = "B+")]
        BPositive,

        [EnumMember(Value = "AB+")]
        ABPositive,

        [EnumMember(Value = "AB-")]
        ABNegative,

        [EnumMember(Value = "A-")]
        ANegative,

        [EnumMember(Value = "B-")]
        BNegative,

        [EnumMember(Value = "O-")]
        ONegative
    }
}
