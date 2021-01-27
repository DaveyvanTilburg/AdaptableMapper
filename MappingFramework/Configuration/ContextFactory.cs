using System.Collections.Generic;
using MappingFramework.Visitors;

namespace MappingFramework.Configuration
{
    public class ContextFactory : IVisitable
    {
        public SourceCreator SourceCreator { get; set; }
        public TargetCreator TargetCreator { get; set; }
        public List<AdditionalSource> AdditionalSources { get; set; }

        public ContextFactory() { }
        
        public ContextFactory(SourceCreator sourceCreator, TargetCreator targetCreator)
        {
            SourceCreator = sourceCreator;
            TargetCreator = targetCreator;
        }

        public ContextFactory(SourceCreator sourceCreator, TargetCreator targetCreator, List<AdditionalSource> additionalSources)
        {
            SourceCreator = sourceCreator;
            TargetCreator = targetCreator;
            AdditionalSources = additionalSources;
        }

        public Context Create(object input, object targetSource)
        {
            var context = new Context();
            var source = SourceCreator.Convert(context, input);
            var target = TargetCreator.Create(context, targetSource);

            context.Source = source;
            context.Target = target;
            var additionalSourceValues = new AdditionalSourceValues();
            if (AdditionalSources != null)
            {
                foreach (AdditionalSource additionalSource in AdditionalSources)
                    additionalSourceValues.AddAdditionalSource(additionalSource, context);
            }

            context.AdditionalSourceValues = additionalSourceValues;

            return context;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(SourceCreator);
            visitor.Visit(TargetCreator);
        }
    }
}