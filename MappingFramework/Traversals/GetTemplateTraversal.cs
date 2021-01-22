using MappingFramework.ContextTypes;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter)), ContextType(ContextType.Target)]
    public interface GetTemplateTraversal
    {
        Template GetTemplate(object target, MappingCaches mappingCache);
    }
}