using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.Model
{
    public sealed class ModelGetValueTraversal : GetValueTraversal, GetValueTraversalPathProperty, ResolvableByTypeId
    {
        public const string _typeId = "9ea49672-2de5-47f5-83c4-0b6fec9432ea";
        public string TypeId => _typeId;

        public ModelGetValueTraversal() { }
        public ModelGetValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(Context context)
        {
            if(!(context.Source is TraversableDataStructure model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#16; source is not of expected type Model", "error", Path);
                return string.Empty;
            }

            var modelPathContainer = PathContainer.Create(Path);

            TraversableDataStructure pathTarget = model.NavigateTo(modelPathContainer.CreatePathQueue());
            if (!pathTarget.IsValid())
                return string.Empty;

            string value = pathTarget.GetValue(modelPathContainer.LastInPath);
            return value;
        }
    }
}