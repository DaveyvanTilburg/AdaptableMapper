using AdaptableMapper.Model;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelSetValueOnPathTraversal : SetFormattedValueTraversal
    {
        public ModelSetValueOnPathTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        protected override void SetValueImplementation(object target, string value)
        {
            if (!(target is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#18; target is not of expected type Model", "error", Path, target);
                return;
            }

            var modelPathContainer = PathContainer.Create(Path);
            ModelBase pathTarget = model.GetOrCreateModel(modelPathContainer.CreatePathQueue());

            pathTarget.SetValue(modelPathContainer.LastInPath, value);
        }
    }
}