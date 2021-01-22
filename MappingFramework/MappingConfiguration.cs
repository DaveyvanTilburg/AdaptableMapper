using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Visitors;

namespace MappingFramework
{
    public sealed class MappingConfiguration : IVisitable
    {
        public ContextFactory ContextFactory { get; set; }
        public ResultObjectConverter ResultObjectConverter { get; set; }
        
        public List<Mapping> Mappings { get; set; }
        public List<MappingScopeComposite> MappingScopeComposites { get; set; }

        private readonly MappingCaches _mappingCaches;

        public MappingConfiguration()
        {
            Mappings = new List<Mapping>();
            MappingScopeComposites = new List<MappingScopeComposite>();
            _mappingCaches = new MappingCaches();
        }

        public MappingConfiguration(List<MappingScopeComposite> mappingScopeComposites, ContextFactory contextFactory, ResultObjectConverter resultObjectConverter) : this()
        {
            MappingScopeComposites = new List<MappingScopeComposite>(mappingScopeComposites ?? new List<MappingScopeComposite>());
            Mappings = new List<Mapping>();
            ContextFactory = contextFactory;
            ResultObjectConverter = resultObjectConverter;
        }

        public MappingConfiguration(List<Mapping> mappings, ContextFactory contextFactory, ResultObjectConverter resultObjectConverter) : this()
        {
            MappingScopeComposites = new List<MappingScopeComposite>();
            Mappings = new List<Mapping>(mappings ?? new List<Mapping>());
            ContextFactory = contextFactory;
            ResultObjectConverter = resultObjectConverter;
        }

        public MappingConfiguration(List<MappingScopeComposite> mappingScopeComposites, List<Mapping> mappings, ContextFactory contextFactory, ResultObjectConverter resultObjectConverter) : this()
        {
            MappingScopeComposites = new List<MappingScopeComposite>(mappingScopeComposites ?? new List<MappingScopeComposite>());
            Mappings = new List<Mapping>(mappings ?? new List<Mapping>());
            ContextFactory = contextFactory;
            ResultObjectConverter = resultObjectConverter;
        }

        public object Map(object source, object targetSource)
        {
            if (!Validate())
                return null;

            Context context = ContextFactory.Create(source, targetSource);

            foreach (Mapping mapping in Mappings)
                mapping.Map(context, _mappingCaches);

            foreach (MappingScopeComposite mappingScopeComposite in MappingScopeComposites)
                mappingScopeComposite.Traverse(context, _mappingCaches);

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

            if (ResultObjectConverter == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#6; ObjectConverter cannot be null", "error"); 
                result = false;
            }

            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(ContextFactory);
            visitor.Visit(ResultObjectConverter);

            if (Mappings != null)
                foreach (Mapping mapping in Mappings)
                    visitor.Visit(mapping);

            if (MappingScopeComposites != null)
                foreach (MappingScopeComposite scope in MappingScopeComposites)
                    visitor.Visit(scope);
        }
    }
}