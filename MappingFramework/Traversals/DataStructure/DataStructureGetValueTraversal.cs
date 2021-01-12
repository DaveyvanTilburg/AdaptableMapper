using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.DataStructure
{
    public sealed class DataStructureGetValueTraversal : GetValueTraversal, GetValueTraversalPathProperty, ResolvableByTypeId
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
            if(!(context.Source is TraversableDataStructure dataStructure))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#16; source is not of expected type TraversableDataStructure", "error", Path);
                return string.Empty;
            }

            var pathContainer = PathContainer.Create(Path);

            TraversableDataStructure pathTarget = dataStructure.NavigateTo(pathContainer.CreatePathQueue());
            if (!pathTarget.IsValid())
                return string.Empty;

            string value = pathTarget.GetValue(pathContainer.LastInPath);
            return value;
        }
    }
}