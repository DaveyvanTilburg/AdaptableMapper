using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdaptableMapper.Xml
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum XmlInterpretation
    {
        Default = 0,
        WithoutNamespace = 1
    }
}