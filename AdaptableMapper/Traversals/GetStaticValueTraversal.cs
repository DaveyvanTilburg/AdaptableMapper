using AdaptableMapper.Configuration;
using AdaptableMapper.ValueMutations.Traversals;

namespace AdaptableMapper.Traversals
{
    public class GetStaticValueTraversal : GetValueTraversal, GetValueStringTraversal
    {
        public string Value { get; set; }

        public GetStaticValueTraversal(string value)
            => Value = value;

        public string GetValue(string value)
        {
            return GetValue(new Context(value, string.Empty));
        }

        public string GetValue(Context context)
        {
            if (string.IsNullOrWhiteSpace(Value))
                Process.ProcessObservable.GetInstance().Raise("GetStaticValueTraversal#1; Value is set to an empty string", "error");

            return Value;
        }
    }
}