using System.Collections.Generic;
using AdaptableMapper.Conditions;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;
using System.Linq;

namespace AdaptableMapper.Compositions
{
    public class GetConditionedListValueTraversal : GetListValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "cddd440f-38d1-49ef-8108-4d412454baed";
        public string TypeId => _typeId;

        public GetConditionedListValueTraversal() { }
        public GetConditionedListValueTraversal(GetListValueTraversal getListValueTraversal, Condition condition)
        {
            GetListValueTraversal = getListValueTraversal;
            Condition = condition;
        }

        public GetListValueTraversal GetListValueTraversal { get; set; }
        public Condition Condition { get; set; }


        public MethodResult<IEnumerable<object>> GetValues(Context context)
        {
            if (!Validate())
                return new NullMethodResult<IEnumerable<object>>();

            MethodResult<IEnumerable<object>> values = GetListValueTraversal.GetValues(context);

            if (values.IsValid)
                values = new MethodResult<IEnumerable<object>>(values.Value.Where(v => Condition.Validate(new Context(v, context.Target))));

            return values;
        }

        private bool Validate()
        {
            bool result = true;

            if (GetListValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetConditionedListValueTraversal#1; {nameof(GetListValueTraversal)} cannot be null", "error");
                result = false;
            }

            if (Condition == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetConditionedListValueTraversal#2; {nameof(Condition)} cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}