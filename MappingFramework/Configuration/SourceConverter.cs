using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface SourceCreator
    {
        object Convert(Context context, object source);
    }
}