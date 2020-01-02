using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.ValueMutations
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface ValueMutation
    {
        string Mutate(Context context, string value);
    }
}