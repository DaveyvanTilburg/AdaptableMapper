using Newtonsoft.Json;

namespace AdaptableMapper
{
    public static class Mapper
    {
        public static string GetMemento(MappingConfiguration mappingConfiguration)
        {
            Formatting indented = Formatting.Indented;
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string serialized = JsonConvert.SerializeObject(mappingConfiguration, indented, settings);
            return serialized;
        }

        public static MappingConfiguration LoadMemento(string memento)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var deserialized = JsonConvert.DeserializeObject<MappingConfiguration>(memento, settings);
            return deserialized;
        }
    }
}