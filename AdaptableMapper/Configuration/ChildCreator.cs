using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;
using Newtonsoft.Json;

namespace AdaptableMapper.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface ChildCreator
    {
        object CreateChild(Template template);
        void AddToParent(Template template, object newChild);
    }
}