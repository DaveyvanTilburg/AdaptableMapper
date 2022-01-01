using System;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using Newtonsoft.Json.Linq;

namespace MappingFramework.Languages.Json.Configuration
{
    [ContentType(ContentType.Json)]
    public sealed class JsonSourceCreator : SourceCreator, ResolvableByTypeId
    {
        public const string _typeId = "bcda7358-80df-4525-86a6-849bd5a5050e";
        public string TypeId => _typeId;

        public JsonSourceCreator() { }

        public object Convert(Context context, object source)
        {
            if (source is not string input)
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