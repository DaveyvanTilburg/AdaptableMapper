namespace AdaptableMapper.Formats.FormatTypes
{
    public abstract class FormatType
    {
        internal abstract string Key { get; }
        public abstract string Format(string source, string formatTemplate);
    }
}