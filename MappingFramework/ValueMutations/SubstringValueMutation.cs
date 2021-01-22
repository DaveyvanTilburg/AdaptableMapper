using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework.ValueMutations
{
    public sealed class SubstringValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "61d3cfbd-3db8-4841-bfd7-65a78520f85d";
        public string TypeId => _typeId;

        public SubstringValueMutation() { }
        public SubstringValueMutation(GetValueStringTraversal getValueStringTraversal)
            => GetValueStringTraversal = getValueStringTraversal;

        public GetValueStringTraversal GetValueStringTraversal { get; set; }

        public string Mutate(Context context, string value)
            => GetValueStringTraversal.GetValue(value);
    }
}