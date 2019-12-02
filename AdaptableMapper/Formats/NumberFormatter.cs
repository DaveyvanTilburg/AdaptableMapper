using System.Linq;

namespace AdaptableMapper.Formats
{
    public class NumberFormatter : Formatter
    {
        public string Separator { get; set; }
        public int NumberOfDecimals { get; set; }

        public NumberFormatter(string separator, int numberOfDecimals)
        {
            Separator = separator;
            NumberOfDecimals = numberOfDecimals;
        }

        public string Format(string source)
        {
            string filteredSource = new string(source.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(Separator))
                return filteredSource;

            if(filteredSource.Length < NumberOfDecimals)
            {
                int missingDigits = NumberOfDecimals - filteredSource.Length;
                return $"0{Separator}{new string('0', missingDigits)}{filteredSource}";
            }

            if (filteredSource.Length == NumberOfDecimals)
                return $"0{Separator}{filteredSource}";

            string left = filteredSource.Substring(0, filteredSource.Length - NumberOfDecimals);
            string right = filteredSource.Substring(filteredSource.Length - NumberOfDecimals, NumberOfDecimals);

            return $"{left}{Separator}{right}";
        }
    }
}