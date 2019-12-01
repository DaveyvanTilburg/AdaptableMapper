namespace AdaptableMapper.Formats.FormatTypes
{
    internal class DateFormatType : DateFormatTypeBase
    {
        protected override string FormatString => "yyyy/MM/dd";
        internal override string Key => "Date";
    }
}