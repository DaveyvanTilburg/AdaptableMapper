using System;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using Newtonsoft.Json.Linq;

namespace MappingFramework.Languages.Json.Configuration
{
    [ContentType(ContentType.Json)]
    public sealed class JsonTargetCreator : TargetCreator, ResolvableByTypeId
    {
        public const string _typeId = "644aec26-82d3-4d49-8a62-bc110c0d613d";
        public string TypeId => _typeId;

        public JsonTargetCreator() { }

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