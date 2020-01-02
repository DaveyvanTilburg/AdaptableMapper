using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface SetValueTraversal
    {
        void SetValue(Context context, MappingCaches mappingCaches, string value);
    }
}