using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdaptableMapper.Conditions
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ListEvaluationOperator
    {
        Any,
        All
    }
}