using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Compositions
{
    public class IfConditionThenAElseBGetValueTraversal : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "1a695959-fa19-482a-b6ee-01556d9d1688";
        public string TypeId => _typeId;

        public IfConditionThenAElseBGetValueTraversal() { }
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
            if(conditionResult)
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