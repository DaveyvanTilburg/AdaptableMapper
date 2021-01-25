using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework.ValueMutations
{
    [ContentType(ContentType.Any)]
    public sealed class SubstringValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "61d3cfbd-3db8-4841-bfd7-65a78520f85d";
        public string TypeId => _typeId;

        public SubstringValueMutation() { }
        public SubstringValueMutation(GetValueStringTraversal getValueStringTraversal)
            => GetValueStringTraversal = getValueStringTraversal;

        public GetValueStringTraversal GetValueStringTraversal { get; set; }

        public string Mutate(Context context, string value)
            => GetValueStringTraversal.GetValue(context, value);
    }
}