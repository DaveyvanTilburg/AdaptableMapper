using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface ResultObjectConverter
    {
        object Convert(object source);
    }
}