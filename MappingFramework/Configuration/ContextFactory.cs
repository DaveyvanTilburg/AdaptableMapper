using System.Collections.Generic;
using MappingFramework.Visitors;

namespace MappingFramework.Configuration
{
    public class ContextFactory : IVisitable
    {
        public ObjectConverter ObjectConverter { get; set; }
        public TargetInstantiator TargetInstantiator { get; set; }
        public List<AdditionalSource> AdditionalSources { get; set; }

        public ContextFactory() { }
        
        public ContextFactory(ObjectConverter objectConverter, TargetInstantiator targetInstantiator)
        {
            ObjectConverter = objectConverter;
            TargetInstantiator = targetInstantiator;
        }

        public ContextFactory(ObjectConverter objectConverter, TargetInstantiator targetInstantiator, List<AdditionalSource> additionalSources)
        {
            ObjectConverter = objectConverter;
            TargetInstantiator = targetInstantiator;
            AdditionalSources = additionalSources;
        }

        public Context Create(object input, object targetSource)
        {
            var additionalSourceValues = new AdditionalSourceValues();
            if (AdditionalSources != null)
            {
                foreach (AdditionalSource additionalSource in AdditionalSources)
                    additionalSourceValues.AddAdditionalSource(additionalSource);
            }

            return new Context(
                ObjectConverter.Convert(input),
                TargetInstantiator.Create(targetSource),
                additionalSourceValues);
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(ObjectConverter);
            visitor.Visit(TargetInstantiator);
        }
    }
}