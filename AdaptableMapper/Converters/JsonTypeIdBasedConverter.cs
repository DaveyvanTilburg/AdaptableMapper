using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Converters
{
    public class JsonTypeIdBasedConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
            => typeof(SerializableByTypeId).IsAssignableFrom(objectType);

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
            TypeCollection typeCollection = GetSerializableByTypeIdTypeCollection();

            Type typeToCreate = typeCollection.GetType(typeId);

            object result = Activator.CreateInstance(typeToCreate);
            return result;
        }

        private TypeCollection GetSerializableByTypeIdTypeCollection()
        {
            Type typeOfRelevantInterface = typeof(SerializableByTypeId);
            IReadOnlyCollection<string> relevantAssemblyNames = RelevantAssemblyCollection.GetAssemblyNames();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Assembly> relevantAssemblies = assemblies.Where(a => relevantAssemblyNames.Any(n => a.FullName.Contains(n)));
            IEnumerable<Type> exposedTypes = relevantAssemblies.SelectMany(a => a.ExportedTypes);

            IEnumerable<Type> typesWithRelevantInterface = exposedTypes.Where(p => !p.IsInterface && typeOfRelevantInterface.IsAssignableFrom(p));
            TypeCollection result = new TypeCollection(typesWithRelevantInterface);
            return result;
        }
    }
}