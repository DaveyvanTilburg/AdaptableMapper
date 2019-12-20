using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.Conditions
{
    public sealed class ListOfConditions : Condition
    {
        public ListOfConditions()
            => Conditions = new List<Condition>();

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