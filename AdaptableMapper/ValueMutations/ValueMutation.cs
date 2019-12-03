using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public interface ValueMutation
    {
        string Mutate(Context context, string source);
    }
}