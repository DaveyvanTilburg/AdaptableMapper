using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Configuration.DataStructure
{
    public sealed class DataStructureObjectConverter : ObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "0ff249bd-3dc5-4125-b130-42fe89cb31eb";
        public string TypeId => _typeId;

        public DataStructureObjectConverter() { }

        public object Convert(object source)
        {
            if (!(source is TraversableDataStructure converted))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#17; source is not of expected type TraversableDataStructure", "error", source);
                return new NullDataStructure();
            }

            return converted;
        }
    }
}