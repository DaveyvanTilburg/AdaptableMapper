using MappingFramework.ContextTypes;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter)), ContextType(ContextType.Target)]
    public interface ResultObjectCreator
    {
        object Convert(object source);
    }
}