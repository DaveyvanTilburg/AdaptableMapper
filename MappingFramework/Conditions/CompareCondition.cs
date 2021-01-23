using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Process;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Conditions
{
    public sealed class CompareCondition : Condition, ResolvableByTypeId, IVisitable
    {
        public const string _typeId = "f8acf126-e297-496d-82c2-0ce9528fa2a2";
        public string TypeId => _typeId;

        public CompareCondition() { }
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
            string valueA = GetValueTraversalSourceValueA.GetValue(context);
            string valueB = GetValueTraversalSourceValueB.GetValue(context);

            bool result = Compare(valueA, CompareOperator, valueB, context);
            return result;
        }

        private static bool Compare(string valueA, CompareOperator compareOperator, string valueB, Context context)
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
                    result = GreaterThan(valueA, valueB, context);
                    break;
                case CompareOperator.LessThan:
                    result = LessThan(valueA, valueB, context);
                    break;
                case CompareOperator.Contains:
                    result = valueA.Contains(valueB);
                    break;
            }

            return result;
        }

        private static bool GreaterThan(string valueA, string valueB, Context context)
        {
            double numericalA = ToDouble(valueA, context);
            double numericalB = ToDouble(valueB, context);

            bool result = numericalA > numericalB;
            return result;
        }

        private static bool LessThan(string valueA, string valueB, Context context)
        {
            double numericalA = ToDouble(valueA, context);
            double numericalB = ToDouble(valueB, context);

            bool result = numericalA < numericalB;
            return result;
        }

        private static double ToDouble(string value, Context context)
        {
            if(!double.TryParse(value, out double result))
            {
                context.AddInformation($"{value} is not numerical", InformationType.Warning);
                return 0;
            }

            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetValueTraversalSourceValueA);
            visitor.Visit(GetValueTraversalSourceValueB);
        }
    }
}