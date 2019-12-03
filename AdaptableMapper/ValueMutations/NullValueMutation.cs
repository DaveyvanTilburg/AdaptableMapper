namespace AdaptableMapper.ValueMutations
{
    public class NullValueMutation : ValueMutation
    {
        public string Mutate(string source)
            => source;
    }
}