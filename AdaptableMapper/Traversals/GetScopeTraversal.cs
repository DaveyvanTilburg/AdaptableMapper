using System.Collections.Generic;

namespace AdaptableMapper.Traversals
{
    public interface GetScopeTraversal
    {
        MethodResult<IEnumerable<object>> GetScope(object source);
    }
}