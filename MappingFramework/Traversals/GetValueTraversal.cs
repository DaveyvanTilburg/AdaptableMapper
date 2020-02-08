using MappingFramework.Configuration;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface GetValueTraversal
    {
        string GetValue(Context context);
    }
}