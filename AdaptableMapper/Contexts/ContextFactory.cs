namespace AdaptableMapper.Contexts
{
    public class ContextFactory
    {
        public ObjectConverter ObjectConverter { get; set; }
        public TargetInstantiator TargetInstantiator { get; set; }

        public ContextFactory(ObjectConverter objectConverter, TargetInstantiator targetInstantiator)
        {
            ObjectConverter = objectConverter;
            TargetInstantiator = targetInstantiator;
        }

        internal Context Create(object source)
        {
            return new Context(source: ObjectConverter.Convert(source), target: TargetInstantiator.Create());
        }
    }
}