using System.Linq;
using MappingFramework.Configuration;
using MappingFramework.Converters;

namespace MappingFramework.ValueMutations
{
    public sealed class NumberValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "1c8deaa9-d1c8-4ecc-8344-5eb8909afde1";
        public string TypeId => _typeId;

        public NumberValueMutation() { }
        public NumberValueMutation(string separator, int numberOfDecimals)
        {
            Separator = separator;
            NumberOfDecimals = numberOfDecimals;
        }

        public string Separator { get; set; }
        public int NumberOfDecimals { get; set; }

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