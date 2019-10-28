using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public class AdaptableSet : SetTraversal
    {
        public AdaptableSet(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is Adaptable adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return;
            }

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(Path);
            Adaptable pathTarget = adaptable.GetOrCreateAdaptable(adaptablePathContainer.CreatePathQueue());

            pathTarget.SetValue(adaptablePathContainer.PropertyName, value);
        }
    }
}