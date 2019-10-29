using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
{
    public sealed class ModelTraversalThis : Traversal
    {
        public object Traverse(object target)
        {
            return target;
        }
    }
}