using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface ResultObjectConverter
    {
        object Convert(object source);
    }
}