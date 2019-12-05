using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdaptableMapper.Conditions
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CompareOperator
    {
        Equals = 0,
        NotEquals = 1
    }
}