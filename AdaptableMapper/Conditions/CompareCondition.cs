using AdaptableMapper.Traversals;

namespace AdaptableMapper.Conditions
{
    public sealed class CompareCondition : Condition
    {
        public GetValueTraversal GetValueTraversalSourceValueA { get; set; }
        public GetValueTraversal GetValueTraversalSourceValueB { get; set; }
        public CompareOperator CompareOperator { get; set; }

        public CompareCondition(GetValueTraversal getValueTraversalSourceValueA, GetValueTraversal getValueTraversalSourceValueB, CompareOperator compareOperator)
        {
            GetValueTraversalSourceValueA = getValueTraversalSourceValueA;
            GetValueTraversalSourceValueB = getValueTraversalSourceValueB;
            CompareOperator = compareOperator;
        }

        public bool Validate(object source)
        {
            if (!ValidateState())
                return false;

            string valueA = GetValueTraversalSourceValueA.GetValue(source);
            string valueB = GetValueTraversalSourceValueB.GetValue(source);

            bool result = Compare(valueA, CompareOperator, valueB);
            return result;
        }

        private static bool Compare(string valueA, CompareOperator compareOperator, string valueB)
        {
            bool result = false;

            switch (compareOperator)
            {
                case CompareOperator.Equals:
                    result = valueA.Equals(valueB);
                    break;
                case CompareOperator.NotEquals:
                    result = !valueA.Equals(valueB);
                    break;
            }

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