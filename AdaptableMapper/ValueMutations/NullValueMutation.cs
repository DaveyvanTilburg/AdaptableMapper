using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public class NullValueMutation : ValueMutation
    {
        public string Mutate(Context context, string source)
            => source;
    }
}