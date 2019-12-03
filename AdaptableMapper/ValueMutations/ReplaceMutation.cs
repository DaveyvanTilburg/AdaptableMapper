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
            string valueToReplace = GetValueTraversalValueToReplace.GetValue(context.Source);
            string newValue = GetValueTraversalNewValue.GetValue(context.Source);

            string result = source.Replace(valueToReplace, newValue);
            return result;
        }
    }
}