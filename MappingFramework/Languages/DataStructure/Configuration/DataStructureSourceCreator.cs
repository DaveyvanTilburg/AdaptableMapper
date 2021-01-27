using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.Languages.DataStructure.Configuration
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureSourceCreator : SourceCreator, ResolvableByTypeId
    {
        public const string _typeId = "0ff249bd-3dc5-4125-b130-42fe89cb31eb";
        public string TypeId => _typeId;

        public DataStructureSourceCreator() { }

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