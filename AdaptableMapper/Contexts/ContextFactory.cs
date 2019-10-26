namespace AdaptableMapper.Contexts
{
    public class ContextFactory
    {
        public ObjectConverter SourceConverter { get; set; }
        public TargetInstantiator TargetInstantiator { get; set; }

        public ContextFactory(ObjectConverter sourceConverter, TargetInstantiator targetInstantiator)
        {
            SourceConverter = sourceConverter;
            TargetInstantiator = targetInstantiator;
        }

        public Context Create(object source, object targetInstantiationMaterial)
        {
            return new Context(source: SourceConverter.Convert(source), target: TargetInstantiator.Create(targetInstantiationMaterial));
        }
    }
}