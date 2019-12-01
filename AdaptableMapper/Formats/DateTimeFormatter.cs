using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Formats
{
    public class DateTimeFormatter : Formatter
    {
        private readonly IReadOnlyCollection<FormatType> _formatTypes;

        public IReadOnlyCollection<string> FormatTypes => _formatTypes.Select(f => f.Key).ToList();
        public string FormatType { get; set; }
        public DateTimeFormatter()
        {
            _formatTypes = new List<FormatType>
            {
                new DateFormatType(), //Todo implement flyweight pattern
                new DateISO8601FormatType() //Todo implement flyweight pattern
            };
        }

        public DateTimeFormatter(string formatType) : this()
        {
            FormatType = formatType;
        }

        public string Format(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                Process.ProcessObservable.GetInstance().Raise("Format#2; source is empty", "warning");
                return source;
            }

            FormatType formatType = GetFormatType();

            string result = formatType?.Format(source) ?? source;
            return result;
        }

        private FormatType GetFormatType()
        {
            FormatType result = _formatTypes.FirstOrDefault(f => f.Is(FormatType));

            if (result == null)
                Process.ProcessObservable.GetInstance().Raise($"Format#1; FormatType: {FormatType} is not valid for :{nameof(DateTimeFormatter)}", "error");

            return result;
        }
    }
}