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
            if (!ValidateState())
                return false;

            string sourceValue = GetValueTraversalSource.GetValue(source);
            string targetValue = GetValueTraversalTarget.GetValue(source);

            bool result = sourceValue.Equals(targetValue);
            return result;
        }

        private bool ValidateState()
        {
            bool result = true;

            if (GetValueTraversalSource == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"EqualsCondition#1; {nameof(GetValueTraversalSource)} is null", "error");
                result = false;
            }

            if (GetValueTraversalTarget == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"EqualsCondition#2; {nameof(GetValueTraversalTarget)} is null", "error");
                result = false;
            }

            return result;
        }
    }
}