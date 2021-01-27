using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.DataStructure
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureSetValueOnPathTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "05f91570-3786-40ce-8edd-b33e36762804";
        public string TypeId => _typeId;

        public DataStructureSetValueOnPathTraversal() { }
        public DataStructureSetValueOnPathTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(Context context, string value)
        {
            TraversableDataStructure dataStructure = (TraversableDataStructure)context.Target;

            var pathContainer = PathContainer.Create(Path);
            TraversableDataStructure pathTarget = dataStructure.GetOrCreate(pathContainer.CreatePathQueue(), context);

            pathTarget.SetValue(pathContainer.LastInPath, value);
        }
    }
}