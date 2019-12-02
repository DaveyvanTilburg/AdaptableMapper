namespace AdaptableMapper.Formats.FormatTypes
{
    public class NullFormatType : FormatType
    {
        public const string NullFormatTypeKey = "None";
        internal override string Key => NullFormatTypeKey;

        public override string Format(string source, string formatTemplate)
        {
            return source;
        }
    }
}