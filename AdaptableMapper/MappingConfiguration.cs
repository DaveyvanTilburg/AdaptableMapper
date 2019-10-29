using AdaptableMapper.Contexts;

namespace AdaptableMapper
{
    public sealed class MappingConfiguration
    {
        public MappingScope MappingScope { get; set; }
        public ContextFactory ContextFactory { get; set; }
        public ObjectConverter ResultConverter { get; set; }

        public MappingConfiguration(MappingScope mappingScope, ContextFactory contextFactory, ObjectConverter resultConverter)
        {
            MappingScope = mappingScope;
            ContextFactory = contextFactory;
            ResultConverter = resultConverter;
        }

        public object Map(object source)
        {
            Context context = ContextFactory.Create(source);
            MappingScope.Traverse(context);

            object result = ResultConverter.Convert(context.Target);
            return result;
        }
    }
}