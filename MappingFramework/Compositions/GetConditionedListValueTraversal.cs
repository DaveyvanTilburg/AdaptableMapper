using System.Collections.Generic;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using System.Linq;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    public class GetConditionedListValueTraversal : GetListValueTraversal, ResolvableByTypeId, IVisitable
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
            MethodResult<IEnumerable<object>> values = GetListValueTraversal.GetValues(context);

            if (!values.IsValid)
                return values;

            if (Condition != null)
                values = new MethodResult<IEnumerable<object>>(values.Value.Where(v => Condition.Validate(new Context(v, context.Target, context.AdditionalSourceValues))));

            if (DistinctByGetValueTraversal != null)
                values = new MethodResult<IEnumerable<object>>(
                    values.Value.GroupBy(e => DistinctByGetValueTraversal.GetValue(new Context(e, context.Target, context.AdditionalSourceValues)))
                    .Select(e => e.First())
                    .ToList());

            return values;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetListValueTraversal);
            visitor.Visit(Condition);
            visitor.Visit(DistinctByGetValueTraversal);
        }
    }
}