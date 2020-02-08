using MappingFramework.Configuration;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.ValueMutations
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface ValueMutation
    {
        string Mutate(Context context, string value);
    }
}