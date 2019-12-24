using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public sealed class ToLowerValueMutation : ValueMutation
    {
        public string Mutate(Context context, string value)
        {
            return value.ToLower();
        }
    }
}