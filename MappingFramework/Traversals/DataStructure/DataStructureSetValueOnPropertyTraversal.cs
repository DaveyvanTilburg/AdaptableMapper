using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.DataStructure
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureSetValueOnPropertyTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "12151374-07cd-4a74-93e3-550e69ce61c0";
        public string TypeId => _typeId;

        public DataStructureSetValueOnPropertyTraversal() { }
        public DataStructureSetValueOnPropertyTraversal(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        public void SetValue(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is TraversableDataStructure dataStructure))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#19; target is not of expected type DataStructure", "error", PropertyName, context.Target);
                return;
            }

            dataStructure.SetValue(PropertyName, value);
        }
    }
}