using AdaptableMapper.Traversals;

namespace AdaptableMapper.Conditions
{
    public class EqualsCondition : Condition
    {
        public GetValueTraversal GetValueTraversalSource { get; set; }
        public GetValueTraversal GetValueTraversalTarget { get; set; }

        public EqualsCondition(GetValueTraversal getValueTraversalSource, GetValueTraversal getValueTraversalTarget)
        {
            GetValueTraversalSource = getValueTraversalSource;
            GetValueTraversalTarget = getValueTraversalTarget;
        }

        public bool Validate(object source)
        {
            string sourceValue = GetValueTraversalSource.GetValue(source);
            string targetValue = GetValueTraversalTarget.GetValue(source);

            bool result = sourceValue.Equals(targetValue);
            return result;
        }
    }
}