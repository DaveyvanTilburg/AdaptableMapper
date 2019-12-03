using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.ValueMutations
{
    public class ReplaceMutation : ValueMutation
    {
        public GetValueTraversal GetValueTraversalValueToReplace { get; set; }
        public GetValueTraversal GetValueTraversalNewValue { get; set; }

        public ReplaceMutation(GetValueTraversal getValueTraversalValueToReplace, GetValueTraversal getValueTraversalNewValue)
        {
            GetValueTraversalValueToReplace = getValueTraversalValueToReplace;
            GetValueTraversalNewValue = getValueTraversalNewValue;
        }

        public string Mutate(Context context, string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                Process.ProcessObservable.GetInstance().Raise("ReplaceMutation#1; cannot mutate an empty string", "warning");
                return source;
            }

            string valueToReplace = GetValueTraversalValueToReplace.GetValue(context.Source);
            if (string.IsNullOrEmpty(valueToReplace))
                return source;

            string newValue = GetValueTraversalNewValue.GetValue(context.Source);
            if (string.IsNullOrEmpty(valueToReplace))
                return source;

            string result = source.Replace(valueToReplace, newValue);
            return result;
        }
    }
}