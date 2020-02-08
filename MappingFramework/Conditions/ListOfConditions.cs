using System.Collections.Generic;
using System.Linq;
using MappingFramework.Configuration;
using MappingFramework.Converters;

namespace MappingFramework.Conditions
{
    public sealed class ListOfConditions : Condition, ResolvableByTypeId
    {
        public const string _typeId = "4d69f541-883c-46c5-8a31-a9ace37358f9";
        public string TypeId => _typeId;

        public ListOfConditions() { }
        public ListOfConditions(ListEvaluationOperator listEvaluationOperator)
        {
            ListEvaluationOperator = listEvaluationOperator;
            Conditions = new List<Condition>();
        }
        public ListOfConditions(ListEvaluationOperator listEvaluationOperator, IEnumerable<Condition> conditions)
        {
            ListEvaluationOperator = listEvaluationOperator;
            Conditions = new List<Condition>(conditions);
        }

        public ListEvaluationOperator ListEvaluationOperator { get; set; }
        public List<Condition> Conditions { get; set; }

        public bool Validate(Context context)
        {
            if (!Validate())
                return false;

            bool result = false;
            switch (ListEvaluationOperator)
            {
                case ListEvaluationOperator.Any:
                    result = Conditions.Any(c => c.Validate(context));
                    break;
                case ListEvaluationOperator.All:
                    result = Conditions.All(c => c.Validate(context));
                    break;
            }

            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if ((Conditions?.Any() ?? false) == false)
            {
                Process.ProcessObservable.GetInstance().Raise($"ListOfConditions#1; {nameof(Conditions)} is empty", "error");
                result = false;
            }

            return result;
        }
    }
}