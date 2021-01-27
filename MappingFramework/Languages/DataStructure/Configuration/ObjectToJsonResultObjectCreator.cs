using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Languages.DataStructure.Configuration
{
    [ContentType(ContentType.DataStructure)]
    public sealed class ObjectToJsonResultObjectCreator : ResultObjectCreator, ResolvableByTypeId
    {
        public const string _typeId = "5e251dd5-ba6e-4de4-8973-8ed67d0e1991";
        public string TypeId => _typeId;

        public ObjectToJsonResultObjectCreator() { }

        public object Convert(object source)
            => JsonConvert.SerializeObject(source);
    }
}