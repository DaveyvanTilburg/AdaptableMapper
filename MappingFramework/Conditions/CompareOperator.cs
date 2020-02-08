using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MappingFramework.Conditions
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CompareOperator
    {
        Equals = 0,
        NotEquals = 1,
        GreaterThan = 2,
        LessThan = 3,
        Contains = 4
    }
}