using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Configuration.DataStructure
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureObjectConverter : ObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "0ff249bd-3dc5-4125-b130-42fe89cb31eb";
        public string TypeId => _typeId;

        public DataStructureObjectConverter() { }

        public object Convert(Context context, object source)
        {
            if (!(source is TraversableDataStructure converted))
            {
                context.InvalidType(source, typeof(TraversableDataStructure));
                return new NullDataStructure();
            }

            return converted;
        }
    }
}