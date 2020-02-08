namespace MappingFramework.Configuration
{
    public sealed class Context
    {
        public object Source { get; set; }
        public object Target { get; set; }

        public Context(
            object source, 
            object target)
        {
            Source = source;
            Target = target;
        }
    }
}