using AdaptableMapper.Conditions;
using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Compositions
{
    public class IfConditionThenAElseBGetValueTraversal : GetValueTraversal
    {
        public IfConditionThenAElseBGetValueTraversal(Condition condition, GetValueTraversal getValueTraversalA, GetValueTraversal getValueTraversalB)
        {
            Condition = condition;
            GetValueTraversalA = getValueTraversalA;
            GetValueTraversalB = getValueTraversalB;
        }

        public Condition Condition { get; set; }
        public GetValueTraversal GetValueTraversalA { get; set; }
        public GetValueTraversal GetValueTraversalB { get; set; }

        public string GetValue(Context context)
        {
            if (!Validate())
                return string.Empty;

            bool conditionResult = Condition.Validate(context);
            if (conditionResult)
                return GetValueTraversalA.GetValue(context);

            return GetValueTraversalB.GetValue(context);
        }

        private bool Validate()
        {
            bool result = true;

            if (Condition == null)
            {
                Process.ProcessObservable.GetInstance().Raise("IfConditionThenAElseBGetValueTraversal#1; Condition cannot be null", "error");
                result = false;
            }

            if (GetValueTraversalA == null)
            {
                Process.ProcessObservable.GetInstance().Raise("IfConditionThenAElseBGetValueTraversal#2; GetValueTraversalA cannot be null", "error");
                result = false;
            }

            if (GetValueTraversalB == null)
            {
                Process.ProcessObservable.GetInstance().Raise("IfConditionThenAElseBGetValueTraversal#3; GetValueTraversalB cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}