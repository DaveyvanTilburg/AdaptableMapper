using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public sealed class NullValueMutation : ValueMutation
    {
        public string Mutate(Context context, string value)
            => value;
    }
}