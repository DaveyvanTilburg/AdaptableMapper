using AdaptableMapper.Contexts;

namespace AdaptableMapper
{
    public sealed class MappingConfiguration
    {
        public MappingScopeComposite ScopeTraversalComposite { get; set; }
        public ContextFactory ContextFactory { get; set; }
        public ObjectConverter ResultConverter { get; set; }

        public MappingConfiguration(MappingScopeComposite scopeTraversalComposite, ContextFactory contextFactory)
        {
            ScopeTraversalComposite = scopeTraversalComposite;
            ContextFactory = contextFactory;
        }
    }
}