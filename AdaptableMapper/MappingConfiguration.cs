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
            if (source == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#1; Argument cannot be null for MappingConfiguration.Map(string)", "error");
                return null;
            }

            if (!Validate())
                return null;

            Context context = ContextFactory.Create(source);
            MappingScope.Traverse(context);

            object result = ObjectConverter.Convert(context.Target);
            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if (ContextFactory == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#2; ContextFactory cannot be null", "error"); 
                result = false;
            }

            if (MappingScope == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#5; MappingScope cannot be null", "error"); 
                result = false;
            }

            if (ObjectConverter == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#6; ObjectConverter cannot be null", "error"); 
                result = false;
            }

            return result;
        }
    }
}