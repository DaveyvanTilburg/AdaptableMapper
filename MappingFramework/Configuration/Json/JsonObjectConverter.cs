using Newtonsoft.Json.Linq;
using System;
using MappingFramework.Converters;

namespace MappingFramework.Configuration.Json
{
    public sealed class JsonObjectConverter : ObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "bcda7358-80df-4525-86a6-849bd5a5050e";
        public string TypeId => _typeId;

        public JsonObjectConverter() { }

        public object Convert(object source)
        {
            if (!(source is string input))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#12; Source is not of expected type String", "error", source?.GetType().Name);
                return string.Empty;
            }

            JToken jToken;
            try
            {
                jToken = JToken.Parse(input);
            }
            catch (Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#13; Source could not be parsed to JToken", "error", source, exception.GetType().Name, exception.Message);
                jToken = new JObject();
            }

            return jToken;
        }
    }
}