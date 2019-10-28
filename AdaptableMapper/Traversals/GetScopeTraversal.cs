using System.Collections.Generic;

namespace AdaptableMapper.Traversals
{
    public interface GetScopeTraversal
    {
        IEnumerable<object> GetScope(object source);
    }
}