using System;

namespace AdaptableMapper.Formats
{
    public class DateFormatter : Formatter
    {
        public string FormatTemplate { get; set; }

        public DateFormatter(string formatTemplate)
            => FormatTemplate = formatTemplate;

        public string Format(string source)
        {
            if (!DateTime.TryParse(source, out DateTime sourceDateTime))
            {
                Process.ProcessObservable.GetInstance().Raise("DateFormatter#1; source is not a valid date", "warning", this.GetType().Name);
                return source;
            }

            return sourceDateTime.ToString(FormatTemplate);
        }
    }
}