using System;

namespace AdaptableMapper.Formats.FormatTypes
{
    internal class DateFormatType : FormatType
    {
        public const string DateFormatTypeKey = "Date";

        internal override string Key => DateFormatTypeKey;

        public override string Format(string source, string formatTemplate)
        {
            if (!DateTime.TryParse(source, out DateTime sourceDateTime))
            {
                Process.ProcessObservable.GetInstance().Raise("Format#3; source is not a valid date", "warning", this.GetType().Name);
                return source;
            }

            return sourceDateTime.ToString(formatTemplate);
        }
    }
}