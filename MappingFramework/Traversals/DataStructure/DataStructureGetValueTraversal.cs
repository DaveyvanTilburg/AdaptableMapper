using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.DataStructure
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureGetValueTraversal : GetSearchPathValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "9ea49672-2de5-47f5-83c4-0b6fec9432ea";
        public string TypeId => _typeId;

        public DataStructureGetValueTraversal() { }
        public DataStructureGetValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(Context context)
        {
            TraversableDataStructure dataStructure = (TraversableDataStructure)context.Source;

            var pathContainer = PathContainer.Create(Path);

            TraversableDataStructure pathTarget = dataStructure.NavigateTo(pathContainer.CreatePathQueue(), context);
            if (!pathTarget.IsValid())
                return string.Empty;

            string value = pathTarget.GetValue(pathContainer.LastInPath, context);
            return value;
        }
    }
}