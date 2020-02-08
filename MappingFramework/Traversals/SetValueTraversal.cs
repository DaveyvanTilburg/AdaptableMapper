using MappingFramework.Configuration;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface SetValueTraversal
    {
        void SetValue(Context context, MappingCaches mappingCaches, string value);
    }
}