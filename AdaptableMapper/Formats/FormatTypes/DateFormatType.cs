using System;

namespace AdaptableMapper.Formats.FormatTypes
{
    internal class DateFormatType : FormatType
    {
        internal override string Key => "Date";

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