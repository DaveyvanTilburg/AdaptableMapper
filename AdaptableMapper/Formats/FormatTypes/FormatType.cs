namespace AdaptableMapper.Formats.FormatTypes
{
    public abstract class FormatType
    {
        internal abstract string Key { get; }

        public bool Is(string type)
            => Key.Equals(type);

        public abstract string Format(string source);
    }
}