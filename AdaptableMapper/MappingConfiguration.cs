using AdaptableMapper.Contexts;
using AdaptableMapper.Traversals;

namespace AdaptableMapper
{
    public class MappingConfiguration
    {
        public ScopeTraversalComposite ScopeTraversalComposite { get; set; }
        public ContextFactory ContextFactory { get; set; }
    }
}