using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is string input))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type String");
                return string.Empty;
            }

            JToken jToken = JToken.Parse(input);
            return jToken;
        }
    }
}