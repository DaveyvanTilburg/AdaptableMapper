using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MappingFramework.Converters
{
    public class JsonTypeIdBasedConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
            => typeof(ResolvableByTypeId).IsAssignableFrom(objectType);

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
            => throw new InvalidOperationException("Use default serialization.");

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var jsonToken = JToken.Load(reader);

            if (jsonToken.Type == JTokenType.Null)
                return null;

            string typeId = jsonToken["TypeId"].Value<string>();

            object result = CreateObjectByTypeId(typeId);
            serializer.Populate(jsonToken.CreateReader(), result);

            return result;
        }

        private object CreateObjectByTypeId(string typeId)
        {
            Type typeToCreate = ResolvableTypeIdCollection.GetType(typeId);

            object result = Activator.CreateInstance(typeToCreate);
            return result;
        }
    }
}