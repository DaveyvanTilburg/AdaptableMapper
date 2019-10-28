namespace AdaptableMapper.Contexts
{
    internal sealed class Context
    {
        public object Source { get; set; }
        public object Target { get; set; }

        internal Context(
            object source, 
            object target)
        {
            Source = source;
            Target = target;
        }
    }
}