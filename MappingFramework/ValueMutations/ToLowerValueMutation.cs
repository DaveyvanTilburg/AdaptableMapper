using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.ValueMutations
{
    [ContentType(ContentType.Any)]
    public sealed class ToLowerValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "147d68e9-2b97-44d6-bfe4-e92520541dab";
        public string TypeId => _typeId;

        public ToLowerValueMutation() { }

        public string Mutate(Context context, string value)
            => value.ToLower();
    }
}