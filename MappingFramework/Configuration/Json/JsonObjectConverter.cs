using Newtonsoft.Json.Linq;
using System;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.Configuration.Json
{
    [ContentType(ContentType.Json)]
    public sealed class JsonObjectConverter : ObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "bcda7358-80df-4525-86a6-849bd5a5050e";
        public string TypeId => _typeId;

        public JsonObjectConverter() { }

        public object Convert(Context context, object source)
        {
            if (!(source is string input))
            {
                context.InvalidInput(source, typeof(string));
                return new JObject();
            }

            JToken jToken;
            try
            {
                jToken = JToken.Parse(input);
            }
            catch (Exception exception)
            {
                context.OperationFailed(this, exception);
                jToken = new JObject();
            }

            return jToken;
        }
    }
}