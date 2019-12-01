namespace AdaptableMapper.Formats.FormatTypes
{
    internal class DateISO8601FormatType : DateFormatTypeBase
    {
        public const string DateISO8601FormatTypeKey = "ISO8601";
        internal override string Key => DateISO8601FormatTypeKey;
        protected override string FormatString => "o";
    }
}