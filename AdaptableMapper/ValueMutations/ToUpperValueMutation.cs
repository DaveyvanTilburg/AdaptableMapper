using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;

namespace AdaptableMapper.ValueMutations
{
    public sealed class ToUpperValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "cf5c36e2-e1aa-45d7-b8b4-d22609e60b69";
        public string TypeId => _typeId;

        public ToUpperValueMutation() { }
        public string Mutate(Context context, string value)
        {
            return value.ToUpper();
        }
    }
}