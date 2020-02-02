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

        public GetConditionedListValueTraversal(GetListValueTraversal getListValueTraversal, GetValueTraversal distinctByGetValueTraversal)
        {
            GetListValueTraversal = getListValueTraversal;
            DistinctByGetValueTraversal = distinctByGetValueTraversal;
        }

        public GetConditionedListValueTraversal(GetListValueTraversal getListValueTraversal, Condition condition, GetValueTraversal distinctByGetValueTraversal)
        {
            GetListValueTraversal = getListValueTraversal;
            Condition = condition;
            DistinctByGetValueTraversal = distinctByGetValueTraversal;
        }

        public GetListValueTraversal GetListValueTraversal { get; set; }
        public Condition Condition { get; set; }
        public GetValueTraversal DistinctByGetValueTraversal { get; set; }

        public MethodResult<IEnumerable<object>> GetValues(Context context)
        {
            if (!Validate())
                return new NullMethodResult<IEnumerable<object>>();

            MethodResult<IEnumerable<object>> values = GetListValueTraversal.GetValues(context);

            if (!values.IsValid)
                return values;

            if (Condition != null)
                values = new MethodResult<IEnumerable<object>>(values.Value.Where(v => Condition.Validate(new Context(v, context.Target))));

            if (DistinctByGetValueTraversal != null)
                values = new MethodResult<IEnumerable<object>>(
                    values.Value.GroupBy(e => DistinctByGetValueTraversal.GetValue(new Context(e, context.Target)))
                    .Select(e => e.First())
                    .ToList());

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

            if (Condition == null && DistinctByGetValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetConditionedListValueTraversal#2; Either {nameof(Condition)} or {nameof(DistinctByGetValueTraversal)} should be used", "error");
                result = false;
            }

            return result;
        }
    }
}