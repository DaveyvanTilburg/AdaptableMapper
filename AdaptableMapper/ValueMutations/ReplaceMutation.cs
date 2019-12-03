using AdaptableMapper.Traversals;

namespace AdaptableMapper.ValueMutations
{
    public class ReplaceMutation : ValueMutation
    {
        public GetValueTraversal GetValueTraversalValue { get; set; }
        public GetValueTraversal GetValueTraversalReplaceValue { get; set; }

        public string Mutate(string source)
        {
            return source;
        }
    }
}