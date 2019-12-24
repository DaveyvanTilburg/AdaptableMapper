using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public sealed class ToUpperValueMutation : ValueMutation
    {
        public string Mutate(Context context, string value)
        {
            return value.ToUpper();
        }
    }
}