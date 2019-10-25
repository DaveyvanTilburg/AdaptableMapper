using System.Collections.Generic;

namespace XPathSerialization.Traversions
{
    public abstract class ScopeTraversion
    {
        public List<ScopeTraversion> Children { get; set; }
        public List<Map> Mappings { get; set; }

        public abstract void Traverse(Context context);
    }
}