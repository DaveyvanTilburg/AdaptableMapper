namespace AdaptableMapper
{
    public class MethodResult <T>
    {
        public T Value { get; }

        public virtual bool IsValid => true;

        protected MethodResult() { }

        public MethodResult(T value)
        {
            Value = value;
        }
    }
}