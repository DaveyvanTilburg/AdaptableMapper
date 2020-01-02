using System.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;

namespace AdaptableMapper.ValueMutations
{
    public class CreateSeparatedRangeFromNumberValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "40b770ff-d795-434a-a0a6-bb53bb6f5163";
        public string TypeId => _typeId;

        public CreateSeparatedRangeFromNumberValueMutation() { }
        public CreateSeparatedRangeFromNumberValueMutation(string separator)
            => Separator = separator;

        public string Separator { get; set; }
        public int StartingNumber { get; set; }

        public string Mutate(Context context, string value)
        {
            if(!int.TryParse(value, out int hits))
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