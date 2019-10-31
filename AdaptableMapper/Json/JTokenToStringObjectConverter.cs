using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JTokenToStringObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is JToken jToken))
            {
                Errors.ErrorObservable.GetInstance().Raise("JSON#25; source is not of expected type JToken", source);
                return new JObject();
            }

            return jToken.ToString();
        }
    }
}