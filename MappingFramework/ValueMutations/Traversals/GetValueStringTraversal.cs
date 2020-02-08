using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.ValueMutations.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface GetValueStringTraversal
    {
        string GetValue(string source);
    }
}