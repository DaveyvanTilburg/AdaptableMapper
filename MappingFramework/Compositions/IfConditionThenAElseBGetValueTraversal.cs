using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    public class IfConditionThenAElseBGetValueTraversal : GetValueTraversal, ResolvableByTypeId, IVisitable
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
            bool conditionResult = Condition.Validate(context);
            if(conditionResult)
                return GetValueTraversalA.GetValue(context);
            
            return GetValueTraversalB.GetValue(context);
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(Condition);
            visitor.Visit(GetValueTraversalA);
            visitor.Visit(GetValueTraversalB);
        }
    }
}