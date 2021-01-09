using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Compositions
{
    public class NullSetValueTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "6008d888-914b-4d36-8a4d-090890d8f6fc";
        public string TypeId => _typeId;

        public NullSetValueTraversal() { }

        public void SetValue(Context context, MappingCaches mappingCaches, string value) { }
    }
}