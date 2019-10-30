using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JTokenToStringObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is JToken jToken))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type JToken");
                return new JObject();
            }

            return jToken.ToString();
        }
    }
}