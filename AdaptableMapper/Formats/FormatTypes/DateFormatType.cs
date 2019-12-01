namespace AdaptableMapper.Formats.FormatTypes
{
    internal class DateFormatType : DateFormatTypeBase
    {
        public const string DateFormatTypeKey = "Date";
        internal override string Key => DateFormatTypeKey;
        protected override string FormatString => "yyyy/MM/dd";
    }
}