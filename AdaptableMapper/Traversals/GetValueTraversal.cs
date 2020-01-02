using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface GetValueTraversal
    {
        string GetValue(Context context);
    }
}