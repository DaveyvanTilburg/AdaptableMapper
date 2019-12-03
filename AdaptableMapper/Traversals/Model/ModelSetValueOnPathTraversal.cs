using AdaptableMapper.Configuration;
using AdaptableMapper.Model;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelSetValueOnPathTraversal : SetMutableValueTraversal
    {
        public ModelSetValueOnPathTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        protected override void SetValueImplementation(Context context, string value)
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