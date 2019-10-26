using System.Collections.Generic;

namespace XPathSerialization.Traversals
{
    public interface GetScopeTraversal
    {
        IEnumerable<object> GetScope(object source);
    }
}