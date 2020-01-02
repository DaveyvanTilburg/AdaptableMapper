using AdaptableMapper.Configuration;
using AdaptableMapper.ValueMutations.Traversals;

namespace AdaptableMapper.ValueMutations
{
    public sealed class SubstringValueMutation : ValueMutation, SerializableByTypeId
    {
        public const string _typeId = "61d3cfbd-3db8-4841-bfd7-65a78520f85d";
        public string TypeId => _typeId;

        public SubstringValueMutation() { }
        public SubstringValueMutation(GetValueStringTraversal getValueStringTraversal)
            => GetValueStringTraversal = getValueStringTraversal;

        public GetValueStringTraversal GetValueStringTraversal { get; set; }

        public string Mutate(Context context, string value)
        {
            if (!Validate())
                return string.Empty;

            string result = GetValueStringTraversal.GetValue(value);
            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if (GetValueStringTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise("SubstringValueMutation#1; GetValueStringTraversal cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}