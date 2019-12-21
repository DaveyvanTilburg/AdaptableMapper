namespace AdaptableMapper.Builder
{
    internal class Visitor
    {
        public Visitor()
            => Result = new MappingConfiguration();

        public MappingConfiguration Result { get; }
        public object Subject { get; set; }
        public Command Command { get; set; }
    }
}