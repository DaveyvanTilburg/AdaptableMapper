using System.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public class CreateSeparatedRangeFromNumberValueMutation : ValueMutation
    {
        public CreateSeparatedRangeFromNumberValueMutation(string separator)
        {
            Separator = separator;
        }

        public string Separator { get; set; }
        public int StartingNumber { get; set; }

        public string Mutate(Context context, string value)
        {
            if (!int.TryParse(value, out int hits))
            {
                Process.ProcessObservable.GetInstance().Raise("CreateSeparatedRangeFromNumberValueMutation#1; source is not numeric", "error", value);
                return string.Empty;
            }

            string[] parts = Enumerable.Range(0 + StartingNumber, hits).Select(n => n.ToString()).ToArray();
            string result = string.Join(Separator, parts);

            return result;
        }
    }
}