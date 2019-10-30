using AdaptableMapper.Traversals;

namespace AdaptableMapper.Json
{
    public sealed class JsonTraversalThis : Traversal
    {
        public object Traverse(object target)
        {
            return target;
        }
    }
}