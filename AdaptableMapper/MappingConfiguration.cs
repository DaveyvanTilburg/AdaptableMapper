using AdaptableMapper.Contexts;

namespace AdaptableMapper
{
    public sealed class MappingConfiguration
    {
        public MappingScope MappingScope { get; set; }
        public ContextFactory ContextFactory { get; set; }
        public ObjectConverter ObjectConverter { get; set; }

        public MappingConfiguration(MappingScope mappingScope, ContextFactory contextFactory, ObjectConverter objectConverter)
        {
            MappingScope = mappingScope;
            ContextFactory = contextFactory;
            ObjectConverter = objectConverter;
        }

        public object Map(object source)
        {
            Context context = ContextFactory.Create(source);
            MappingScope.Traverse(context);

            object result = ObjectConverter.Convert(context.Target);
            return result;
        }
    }
}