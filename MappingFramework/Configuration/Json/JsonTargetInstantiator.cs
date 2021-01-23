using Newtonsoft.Json.Linq;
using System;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.Configuration.Json
{
    [ContentType(ContentType.Json)]
    public sealed class JsonTargetInstantiator : TargetInstantiator, ResolvableByTypeId
    {
        public const string _typeId = "644aec26-82d3-4d49-8a62-bc110c0d613d";
        public string TypeId => _typeId;

        public JsonTargetInstantiator() { }

        public object Create(Context context, object source)
        {
            if (!(source is string template))
            {
                context.InvalidInput(source, typeof(string));
                return new JObject();
            }

            JToken jToken;
            try
            {
                jToken = JToken.Parse(template);
            }
            catch (Exception exception)
            {
                context.OperationFailed(this, exception);
                return new JObject();
            }

            return jToken;
        }
    }
}