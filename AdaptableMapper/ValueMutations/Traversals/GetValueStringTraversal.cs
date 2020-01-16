using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.ValueMutations.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface GetValueStringTraversal
    {
        string GetValue(string source);
    }
}