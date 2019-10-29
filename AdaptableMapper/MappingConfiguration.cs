using AdaptableMapper.Contexts;
using AdaptableMapper.Traversals;

namespace AdaptableMapper
{
    public sealed class MappingConfiguration
    {
        public MappingScopeComposite ScopeTraversalComposite { get; set; }
        public ContextFactory ContextFactory { get; set; }

        public MappingConfiguration(MappingScopeComposite scopeTraversalComposite, ContextFactory contextFactory)
        {
            ScopeTraversalComposite = scopeTraversalComposite;
            ContextFactory = contextFactory;
        }
    }
}