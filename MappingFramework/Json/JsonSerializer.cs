using System;
using Newtonsoft.Json;

namespace MappingFramework.Json
{
    public static class JsonSerializer
    {
        private const Formatting Indented = Formatting.Indented;

        public static string Serialize(object source)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new OrderedContractResolver()
            };

            string serialized = JsonConvert.SerializeObject(source, Indented, settings);
            return serialized;
        }

        public static T Deserialize<T>(string memento)
        {
            var deserialized = JsonConvert.DeserializeObject<T>(memento);
            return deserialized;
        }

        public static object Deserialize(Type type, string memento)
        {
            var deserialized = JsonConvert.DeserializeObject(memento, type);
            return deserialized;
        }
    }
}