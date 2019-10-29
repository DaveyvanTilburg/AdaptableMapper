using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
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
            if (!(target is ModelBase model))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            ModelBase pathTarget = model.NavigateToModel(Path.ToQueue());

            return pathTarget;
        }
    }
}