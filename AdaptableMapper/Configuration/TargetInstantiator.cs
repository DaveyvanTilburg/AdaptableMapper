using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface TargetInstantiator
    {
        object Create(object source);
    }
}