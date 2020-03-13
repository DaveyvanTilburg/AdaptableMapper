using System.Collections.Generic;

namespace MappingFramework.Configuration
{
    public class ContextFactory
    {
        public ObjectConverter ObjectConverter { get; set; }
        public TargetInstantiator TargetInstantiator { get; set; }
        public List<AdditionalSource> AdditionalSources { get; set; }

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
            if (!Validate())
                return new Context(null, null, null);

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

        private bool Validate()
        {
            bool result = true;

            if (ObjectConverter == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#3; ObjectConverter cannot be null", "error");
                result = false;
            }

            if (TargetInstantiator == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#4; TargetInstantiator cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}