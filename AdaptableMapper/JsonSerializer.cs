using Newtonsoft.Json;

namespace AdaptableMapper
{
    public static class JsonSerializer
    {
        public static string Serialize(object source)
        {
            Formatting indented = Formatting.Indented;
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string serialized = JsonConvert.SerializeObject(source, indented, settings);
            return serialized;
        }

        public static T Deserialize<T>(string memento)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var deserialized = JsonConvert.DeserializeObject<T>(memento, settings);
            return deserialized;
        }
    }
}