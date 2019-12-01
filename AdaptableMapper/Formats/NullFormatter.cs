namespace AdaptableMapper.Formats
{
    public class NullFormatter : Formatter
    {
        public string Format(string source)
            => source;
    }
}