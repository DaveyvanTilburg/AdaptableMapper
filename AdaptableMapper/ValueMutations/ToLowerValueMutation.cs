using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;

namespace AdaptableMapper.ValueMutations
{
    public sealed class ToLowerValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "147d68e9-2b97-44d6-bfe4-e92520541dab";
        public string TypeId => _typeId;

        public ToLowerValueMutation() { }

        public string Mutate(Context context, string value)
        {
            return value.ToLower();
        }
    }
}