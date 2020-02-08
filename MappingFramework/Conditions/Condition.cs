using MappingFramework.Configuration;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Conditions
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface Condition
    {
        bool Validate(Context context);
    }
}