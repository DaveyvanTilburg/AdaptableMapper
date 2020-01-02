using AdaptableMapper.Configuration;
using AdaptableMapper.Model;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelSetValueOnPathTraversal : SetMutableValueTraversal, SerializableByTypeId
    {
        public const string _typeId = "05f91570-3786-40ce-8edd-b33e36762804";
        public string TypeId => _typeId;

        public ModelSetValueOnPathTraversal() { }
        public ModelSetValueOnPathTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        protected override void SetValueImplementation(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#18; target is not of expected type Model", "error", Path, context.Target);
                return;
            }

            var modelPathContainer = PathContainer.Create(Path);
            ModelBase pathTarget = model.GetOrCreateModel(modelPathContainer.CreatePathQueue());

            pathTarget.SetValue(modelPathContainer.LastInPath, value);
        }
    }
}