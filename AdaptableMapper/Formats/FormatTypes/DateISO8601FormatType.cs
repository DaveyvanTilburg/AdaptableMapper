namespace AdaptableMapper.Formats.FormatTypes
{
    internal class DateISO8601FormatType : DateFormatTypeBase
    {
        internal override string Key => "ISO8601";
        protected override string FormatString => "o";
    }
}