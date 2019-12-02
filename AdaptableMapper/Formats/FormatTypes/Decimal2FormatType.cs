using System.Linq;

namespace AdaptableMapper.Formats.FormatTypes
{
    internal class Decimal2FormatType : FormatType
    {
        internal override string Key => "Decimal2";

        public override string Format(string source, string formatTemplate)
        {
            string filteredSource = new string(source.Where(char.IsDigit).ToArray());

            if (filteredSource.Length == 0)
                return $"0{formatTemplate}00";

            if (filteredSource.Length == 1)
                return $"0{formatTemplate}0{filteredSource}";

            if (filteredSource.Length == 2)
                return $"0{formatTemplate}{filteredSource}";

            string left = filteredSource.Substring(0, filteredSource.Length - 2);
            string right = filteredSource.Substring(filteredSource.Length - 2, 2);

            return $"{left}{formatTemplate}{right}";
        }
    }
}