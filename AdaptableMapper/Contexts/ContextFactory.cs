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

        internal Context Create(object source)
        {
            return new Context(source: SourceConverter.Convert(source), target: TargetInstantiator.Create());
        }
    }
}