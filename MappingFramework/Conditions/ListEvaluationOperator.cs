using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MappingFramework.Conditions
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ListEvaluationOperator
    {
        Any,
        All
    }
}