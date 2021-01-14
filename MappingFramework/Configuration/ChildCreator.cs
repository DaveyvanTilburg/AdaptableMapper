using MappingFramework.Converters;
using MappingFramework.Traversals;
using Newtonsoft.Json;

namespace MappingFramework.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter)), ContextType(ContextType.Target)]
    public interface ChildCreator
    {
        object CreateChild(Template template);
        void AddToParent(Template template, object newChild);
    }
}