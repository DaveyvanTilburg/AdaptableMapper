using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;
using AdaptableMapper.ValueMutations.Traversals;

namespace AdaptableMapper.ValueMutations
{
    public sealed class ReplaceValueMutation : ValueMutation
    {
        public GetValueStringTraversal GetValueStringTraversal { get; set; }
        public GetValueTraversal GetValueTraversal { get; set; }

        public ReplaceValueMutation(GetValueStringTraversal getValueStringTraversal, GetValueTraversal getValueTraversal)
        {
            GetValueStringTraversal = getValueStringTraversal;
            GetValueTraversal = getValueTraversal;
        }

        public string Mutate(Context context, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Process.ProcessObservable.GetInstance().Raise("ReplaceMutation#1; cannot mutate an empty string", "warning");
                return value;
            }

            string valueToReplace = GetValueStringTraversal.GetValue(value);
            if (string.IsNullOrEmpty(valueToReplace))
                return value;

            string newValue = GetValueTraversal.GetValue(context.Source);
            if (string.IsNullOrEmpty(newValue))
                return value;

            string result = value.Replace(valueToReplace, newValue);
            return result;
        }
    }
}