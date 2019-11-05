using AdaptableMapper.Contexts;
using Newtonsoft.Json.Linq;
using System;

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
            JToken jToken;
            try
            {
                jToken =JToken.Parse(Template);
            }
            catch (Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#20; Template could not be parsed to JToken", "error", exception.GetType().Name, exception.Message);
                return new JObject();
            }

            return jToken;
        }
    }
}