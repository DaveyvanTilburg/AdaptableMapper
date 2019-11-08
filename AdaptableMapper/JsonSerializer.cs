using System;
using Newtonsoft.Json;

namespace AdaptableMapper
{
    public static class JsonSerializer
    {
        private const Formatting Indented = Formatting.Indented;

        public static string Serialize(object source)
        {
            
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string serialized = JsonConvert.SerializeObject(source, Indented, settings);
            return serialized;
        }

        public static T Deserialize<T>(string memento)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var deserialized = JsonConvert.DeserializeObject<T>(memento, settings);
            return deserialized;
        }

        public static object Deserialize(Type type, string memento)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var deserialized = JsonConvert.DeserializeObject(memento, type, settings);
            return deserialized;
        }
    }
}