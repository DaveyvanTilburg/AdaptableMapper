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

        internal Context Create(object input, object targetSource)
        {
            if (!Validate())
                return new Context(null, null);

            return new Context(source: ObjectConverter.Convert(input), target: TargetInstantiator.Create(targetSource));
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