using System;
using System.Globalization;

namespace AdaptableMapper.Formats
{
    public class DateFormatter : Formatter
    {
        public string FormatTemplate { get; set; }

        public DateFormatter(string formatTemplate)
            => FormatTemplate = formatTemplate;

        public string Format(string source)
        {
            if (!DateTime.TryParse(source, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime sourceDateTime))
            {
                Process.ProcessObservable.GetInstance().Raise("DateFormatter#1; source is not a valid date", "warning");
                return source;
            }

            string result;
            try
            {
                result = sourceDateTime.ToString(FormatTemplate);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("DateFormatter#2; source is not a valid date", "error", exception.Message, exception.GetType().Name);
                return source;
            }
            return result;
        }
    }
}