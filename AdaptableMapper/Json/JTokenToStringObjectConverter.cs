using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JTokenToStringObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#25; source is not of expected type JToken", "error", source?.GetType().Name);
                return new JObject();
            }

            return jToken.ToString(Formatting.Indented);
        }
    }
}