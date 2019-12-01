using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Formats.FormatTypes;

namespace AdaptableMapper.Formats
{
    public class GenericFormatter : Formatter
    {
        public IReadOnlyCollection<string> FormatTypes => FormatTypeFactory.GetInstance().GetAllKeys();
        public string FormatType { get; set; }

        public GenericFormatter(string formatType)
            => FormatType = formatType;

        public string Format(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                Process.ProcessObservable.GetInstance().Raise("Format#2; source is empty", "warning");
                return source;
            }

            FormatType formatType = GetFormatType();

            string result = formatType.Format(source);
            return result;
        }

        private FormatType GetFormatType()
        {
            if (!FormatTypes.Contains(FormatType))
            {
                Process.ProcessObservable.GetInstance().Raise($"Format#1; FormatType: {FormatType} is not valid for :{nameof(GenericFormatter)}", "error");
                FormatType = NullFormatType.NullFormatTypeKey;
            }

            FormatType result = FormatTypeFactory.GetInstance().GetFormatType(FormatType);
            return result;
        }
    }
}