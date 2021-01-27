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

        public MappingConfiguration()
        {
            Mappings = new List<Mapping>();
            MappingScopeComposites = new List<MappingScopeComposite>();
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

        public MapResult Map(object source, object targetSource)
        {
            var validation = new CompositionValidationVisitor();
            validation.Visit(this);

            if (validation.Feedback().Count > 0)
                return new MapResult(null, validation.Feedback());

            Context context = ContextFactory.Create(source, targetSource);

            foreach (Mapping mapping in Mappings)
                mapping.Map(context);

            foreach (MappingScopeComposite mappingScopeComposite in MappingScopeComposites)
                mappingScopeComposite.Traverse(context);

            object result = ResultObjectConverter.Convert(context.Target);
            return new MapResult(result, context.Information());
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