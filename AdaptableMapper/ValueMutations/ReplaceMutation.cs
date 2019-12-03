using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;
using AdaptableMapper.ValueMutations.Traversals;

namespace AdaptableMapper.ValueMutations
{
    public class ReplaceMutation : ValueMutation
    {
        public GetValueStringTraveral GetValueStringTraversal { get; set; }
        public GetValueTraversal GetValueTraversalNewValue { get; set; }

        public ReplaceMutation(GetValueStringTraveral getValueStringTraversal, GetValueTraversal getValueTraversalNewValue)
        {
            GetValueStringTraversal = getValueStringTraversal;
            GetValueTraversalNewValue = getValueTraversalNewValue;
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

            string newValue = GetValueTraversalNewValue.GetValue(context.Source);
            if (string.IsNullOrEmpty(newValue))
                return value;

            string result = value.Replace(valueToReplace, newValue);
            return result;
        }
    }
}