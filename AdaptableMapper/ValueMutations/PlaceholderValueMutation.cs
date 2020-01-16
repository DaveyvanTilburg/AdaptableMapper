using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public sealed class PlaceholderValueMutation : ValueMutation
    {
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