using MappingFramework.Converters;
using MappingFramework.DataStructure;
using Newtonsoft.Json;

namespace MappingFramework.Configuration.DataStructure
{
    public sealed class DataStructureToStringObjectConverter : ResultObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "5e251dd5-ba6e-4de4-8973-8ed67d0e1991";
        public string TypeId => _typeId;

        public DataStructureToStringObjectConverter() { }

        public object Convert(object source)
        {
            if (!(source is TraversableDataStructure converted))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#20; source is not of expected type Model", "error", source);
                return new NullDataStructure();
            }

            string result = JsonConvert.SerializeObject(converted);
            return result;
        }
    }
}