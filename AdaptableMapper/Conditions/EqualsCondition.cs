using AdaptableMapper.Traversals;

namespace AdaptableMapper.Conditions
{
    public class EqualsCondition : Condition
    {
        public GetValueTraversal GetValueTraversal { get; set; }
        public string Value { get; set; }

        public EqualsCondition(GetValueTraversal getValueTraversal, string value)
        {
            GetValueTraversal = getValueTraversal;
            Value = value;
        }

        public bool Validate(object source)
        {
            string value = GetValueTraversal.GetValue(source);

            bool result = value.Equals(Value);
            return result;
        }
    }
}