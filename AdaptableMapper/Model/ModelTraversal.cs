using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public sealed class ModelTraversal : Traversal
    {
        public ModelTraversal(string path)
        {
            Path = path;
        }

        private string Path { get; set; }

        public object Traverse(object target)
        {
            if (!(target is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            ModelBase pathTarget = adaptable.NavigateToAdaptable(Path.ToQueue());

            return pathTarget;
        }
    }
}