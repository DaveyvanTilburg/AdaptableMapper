using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;

namespace AdaptableMapper.ValueMutations
{
    public sealed class PlaceholderValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "57c5ed76-0bd0-4634-8b2c-0553075ec80d";
        public string TypeId => _typeId;

        public PlaceholderValueMutation() { }
        public PlaceholderValueMutation(string placeholder)
        {
            _placeholder = placeholder;
        }

        private readonly string _placeholder;

        public string Mutate(Context context, string value)
        {
            string result = string.Format(_placeholder, value);
            return result;
        }
    }
}