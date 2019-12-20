using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Conditions
{
    public sealed class CompareCondition : Condition
    {
        public CompareCondition(GetValueTraversal getValueTraversalSourceValueA, CompareOperator compareOperator, GetValueTraversal getValueTraversalSourceValueB)
        {
            GetValueTraversalSourceValueA = getValueTraversalSourceValueA;
            CompareOperator = compareOperator;
            GetValueTraversalSourceValueB = getValueTraversalSourceValueB;
        }

        public GetValueTraversal GetValueTraversalSourceValueA { get; set; }
        public CompareOperator CompareOperator { get; set; }
        public GetValueTraversal GetValueTraversalSourceValueB { get; set; }

        public bool Validate(Context context)
        {
            if (!ValidateState())
                return false;

            string valueA = GetValueTraversalSourceValueA.GetValue(context);
            string valueB = GetValueTraversalSourceValueB.GetValue(context);

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
                case CompareOperator.GreaterThan:
                    result = GreaterThan(valueA, valueB);
                    break;
                case CompareOperator.LessThan:
                    result = LessThan(valueA, valueB);
                    break;
            }

            return result;
        }

        private static bool GreaterThan(string valueA, string valueB)
        {
            double numericalA = ToDouble(valueA);
            double numericalB = ToDouble(valueB);

            bool result = numericalA > numericalB;
            return result;
        }

        private static bool LessThan(string valueA, string valueB)
        {
            double numericalA = ToDouble(valueA);
            double numericalB = ToDouble(valueB);

            bool result = numericalA < numericalB;
            return result;
        }

        private static double ToDouble(string value)
        {
            if (!double.TryParse(value, out double result))
            {
                Process.ProcessObservable.GetInstance().Raise($"CompareCondition#3; value {value} is not numerical, using value 0", "warning");
                return 0;
            }

            return result;
        }

        private bool ValidateState()
        {
            bool result = true;

            if (GetValueTraversalSourceValueA == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"CompareCondition#1; {nameof(GetValueTraversalSourceValueA)} is null", "error");
                result = false;
            }

            if (GetValueTraversalSourceValueB == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"CompareCondition#2; {nameof(GetValueTraversalSourceValueB)} is null", "error");
                result = false;
            }

            return result;
        }
    }
}