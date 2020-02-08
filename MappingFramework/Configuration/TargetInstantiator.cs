using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface TargetInstantiator
    {
        object Create(object source);
    }
}