using AdaptableMapper.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Configuration.Json
{
    public sealed class JTokenToStringObjectConverter : ResultObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "111821e4-70dd-43b4-9c5d-3738aa4a102c";
        public string TypeId => _typeId;

        public JTokenToStringObjectConverter() { }

        public bool UseIndentation { get; set; } = true;

        public object Convert(object source)
        {
            if (!(source is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#25; source is not of expected type JToken", "error", source?.GetType().Name);
                return new JObject();
            }

            return jToken.ToString(UseIndentation ? Formatting.Indented : Formatting.None);
        }
    }
}