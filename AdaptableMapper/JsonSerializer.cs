using System;
using Newtonsoft.Json;

namespace AdaptableMapper
{
    public static class JsonSerializer
    {
        private const Formatting Indented = Formatting.Indented;

        public static string Serialize(object source)
        {
            string serialized = JsonConvert.SerializeObject(source, Indented);
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