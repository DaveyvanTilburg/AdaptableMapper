using AdaptableMapper.Contexts;

namespace AdaptableMapper
{
    public sealed class MappingConfiguration
    {
        public MappingScopeComposite ScopeTraversalComposite { get; set; }
        public ContextFactory ContextFactory { get; set; }
        public ObjectConverter ResultConverter { get; set; }

        public MappingConfiguration(MappingScopeComposite scopeTraversalComposite, ContextFactory contextFactory, ObjectConverter resultConverter)
        {
            ScopeTraversalComposite = scopeTraversalComposite;
            ContextFactory = contextFactory;
            ResultConverter = resultConverter;
        }

        public object Map(object source)
        {
            Context context = ContextFactory.Create(source);
            ScopeTraversalComposite.Traverse(context);

            object result = ResultConverter.Convert(context.Target);
            return result;
        }
    }
}