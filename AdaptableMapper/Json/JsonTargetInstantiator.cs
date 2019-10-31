using AdaptableMapper.Contexts;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonTargetInstantiator : TargetInstantiator
    {
        public JsonTargetInstantiator(string template)
        {
            Template = template;
        }
        public string Template { get; set; }

        public object Create()
        {
            JToken jToken = JToken.Parse(Template);
            if (jToken == null)
            {
                Errors.ErrorObservable.GetInstance().Raise("JSON#20; Template could not be parsed to JToken", Template);
                return new JObject();
            }

            return jToken;
        }
    }
}