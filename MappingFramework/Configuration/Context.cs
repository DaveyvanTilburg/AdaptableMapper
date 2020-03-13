namespace MappingFramework.Configuration
{
    public sealed class Context
    {
        public object Source { get; set; }
        public object Target { get; set; }
        public AdditionalSourceValues AdditionalSourceValues { get; set; }

        public Context(
            object source,
            object target,
            AdditionalSourceValues additionalSourceValues)
        {
            Source = source;
            Target = target;
            AdditionalSourceValues = additionalSourceValues;
        }
    }
}