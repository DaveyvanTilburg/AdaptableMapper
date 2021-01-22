using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Configuration.DataStructure
{
    [ContentType(ContentType.DataStructure)]
    public sealed class ObjectToJsonResultObjectConverter : ResultObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "5e251dd5-ba6e-4de4-8973-8ed67d0e1991";
        public string TypeId => _typeId;

        public ObjectToJsonResultObjectConverter() { }

        public object Convert(object source)
        {
            string result = JsonConvert.SerializeObject(source);
            return result;
        }
    }
}