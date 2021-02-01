using System.Collections.Generic;

namespace MappingFramework.Traversals
{
    public interface GetScopeTraversal
    {
        MethodResult<IEnumerable<object>> GetScope(object source);
    }
}