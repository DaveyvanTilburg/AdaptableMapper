using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
{
    public sealed class ModelSetValueOnPath : SetValueTraversal
    {
        public ModelSetValueOnPath(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(object target, string value)
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