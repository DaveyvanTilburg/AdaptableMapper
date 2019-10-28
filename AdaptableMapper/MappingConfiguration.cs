using AdaptableMapper.Contexts;
using AdaptableMapper.Traversals;

namespace AdaptableMapper
{
    public class MappingConfiguration
    {
        public ScopeTraversalComposite ScopeTraversalComposite { get; set; }
        public ContextFactory ContextFactory { get; set; }

        public MappingConfiguration(ScopeTraversalComposite scopeTraversalComposite, ContextFactory contextFactory)
        {
            ScopeTraversalComposite = scopeTraversalComposite;
            ContextFactory = contextFactory;
        }
    }
}