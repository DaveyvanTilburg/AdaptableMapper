using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public sealed class ModelSetOnPath : SetValueTraversal
    {
        public ModelSetOnPath(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return;
            }

            var adaptablePathContainer = ModelPathContainer.CreateAdaptablePath(Path);
            ModelBase pathTarget = adaptable.GetOrCreateAdaptable(adaptablePathContainer.CreatePathQueue());

            pathTarget.SetValue(adaptablePathContainer.PropertyName, value);
        }
    }
}