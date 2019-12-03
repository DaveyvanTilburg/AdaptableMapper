using System.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public class NumberValueMutation : ValueMutation
    {
        public string Separator { get; set; }
        public int NumberOfDecimals { get; set; }

        public NumberValueMutation(string separator, int numberOfDecimals)
        {
            Separator = separator;
            NumberOfDecimals = numberOfDecimals;
        }

        public string Mutate(Context context, string value)
        {
            string filteredSource = new string(value.Where(char.IsDigit).ToArray());

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