using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.Conditions
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface Condition
    {
        bool Validate(Context context);
    }
}