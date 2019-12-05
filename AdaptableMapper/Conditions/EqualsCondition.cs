using AdaptableMapper.Traversals;

namespace AdaptableMapper.Conditions
{
    public class EqualsCondition : Condition
    {
        public GetValueTraversal GetValueTraversalSourceValueA { get; set; }
        public GetValueTraversal GetValueTraversalSourceValueB { get; set; }

        public EqualsCondition(GetValueTraversal getValueTraversalSourceValueA, GetValueTraversal getValueTraversalSourceValueB)
        {
            GetValueTraversalSourceValueA = getValueTraversalSourceValueA;
            GetValueTraversalSourceValueB = getValueTraversalSourceValueB;
        }

        public bool Validate(object source)
        {
            if (!ValidateState())
                return false;

            string sourceValue = GetValueTraversalSourceValueA.GetValue(source);
            string targetValue = GetValueTraversalSourceValueB.GetValue(source);

            bool result = sourceValue.Equals(targetValue);
            return result;
        }

        private bool ValidateState()
        {
            bool result = true;

            if (GetValueTraversalSourceValueA == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"EqualsCondition#1; {nameof(GetValueTraversalSourceValueA)} is null", "error");
                result = false;
            }

            if (GetValueTraversalSourceValueB == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"EqualsCondition#2; {nameof(GetValueTraversalSourceValueB)} is null", "error");
                result = false;
            }

            return result;
        }
    }
}