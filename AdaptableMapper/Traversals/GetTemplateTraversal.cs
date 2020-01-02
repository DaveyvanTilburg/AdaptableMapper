using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface GetTemplateTraversal
    {
        Template GetTemplate(object target, MappingCaches mappingCache);
    }
}