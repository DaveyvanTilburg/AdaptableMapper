using Newtonsoft.Json.Linq;
using System;
using MappingFramework.Converters;

namespace MappingFramework.Configuration.Json
{
    public sealed class JsonTargetInstantiator : TargetInstantiator, ResolvableByTypeId
    {
        public const string _typeId = "644aec26-82d3-4d49-8a62-bc110c0d613d";
        public string TypeId => _typeId;

        public JsonTargetInstantiator() { }

        public object Create(object source)
        {
            if (!(source is string template))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#26; Source is not of expected type string", "error", source, source?.GetType().Name);
                return new JObject();
            }

            JToken jToken;
            try
            {
                jToken =JToken.Parse(template);
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