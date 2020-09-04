using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.Model
{
    public sealed class ModelSetValueOnPathTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "05f91570-3786-40ce-8edd-b33e36762804";
        public string TypeId => _typeId;

        public ModelSetValueOnPathTraversal() { }
        public ModelSetValueOnPathTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is TraversableDataStructure model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#18; target is not of expected type Model", "error", Path, context.Target);
                return;
            }

            var modelPathContainer = PathContainer.Create(Path);
            TraversableDataStructure pathTarget = model.GetOrCreate(modelPathContainer.CreatePathQueue());

            pathTarget.SetValue(modelPathContainer.LastInPath, value);
        }
    }
}