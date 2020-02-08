using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface GetTemplateTraversal
    {
        Template GetTemplate(object target, MappingCaches mappingCache);
    }
}