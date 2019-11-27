using System.Collections.Generic;
using AdaptableMapper.Contexts;

namespace AdaptableMapper
{
    public sealed class MappingConfiguration
    {
        public MappingScope MappingScope { get; set; }
        public ContextFactory ContextFactory { get; set; }
        public ResultObjectConverter ResultObjectConverter { get; set; }
        public List<Mapping> Mappings { get;set; }

        public MappingConfiguration()
        {
            Mappings = new List<Mapping>();
        }

        public MappingConfiguration(ContextFactory contextFactory, ResultObjectConverter resultObjectConverter) : this()
        {
            ContextFactory = contextFactory;
            ResultObjectConverter = resultObjectConverter;
        }

        public object Map(object source, object targetSource)
        {
            if (source == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#1; Argument cannot be null for MappingConfiguration.Map(string)", "error");
                return null;
            }

            if (!Validate())
                return null;

            Context context = ContextFactory.Create(source, targetSource);

            foreach (Mapping mapping in Mappings)
                mapping.Map(context);
            
            MappingScope?.Traverse(context);

            object result = ResultObjectConverter.Convert(context.Target);
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

            if (!(MappingScope != null || Mappings?.Count > 0))
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#5; MappingScope or mappings should be used", "error");
                result = false;
            }

            if (ResultObjectConverter == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#6; ObjectConverter cannot be null", "error"); 
                result = false;
            }

            return result;
        }
    }
}