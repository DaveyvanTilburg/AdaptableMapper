using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public sealed class ModelTraversalThis : Traversal
    {
        public object Traverse(object target)
        {
            return target;
        }
    }
}