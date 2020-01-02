using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface ObjectConverter
    {
        object Convert(object source);
    }
}