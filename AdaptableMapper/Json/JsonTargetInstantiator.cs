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
            return jToken;
        }
    }
}