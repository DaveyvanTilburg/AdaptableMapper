using MappingFramework.ContextTypes;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter)), ContextType(ContextType.Target)]
    public interface ResultObjectConverter
    {
        object Convert(object source);
    }
}