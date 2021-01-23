using MappingFramework.ContextTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using Newtonsoft.Json;

namespace MappingFramework.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter)), ContextType(ContextType.Target)]
    public interface ChildCreator
    {
        object CreateChild(Context context, Template template);
        void AddToParent(Context context, Template template, object newChild);
    }
}