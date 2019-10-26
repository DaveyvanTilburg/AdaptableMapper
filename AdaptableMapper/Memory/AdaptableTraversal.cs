using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public class AdaptableTraversal : Traversal
    {
        private string Path { get; set; }

        public object Traverse(object target)
        {
            if (!(target is Adaptable adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return string.Empty;
            }

            Adaptable pathTarget = adaptable.NavigateToAdaptable(Path.ToQueue());

            return pathTarget;
        }
    }
}