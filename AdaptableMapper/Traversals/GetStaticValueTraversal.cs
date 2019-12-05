using AdaptableMapper.ValueMutations.Traversals;

namespace AdaptableMapper.Traversals
{
    public class GetStaticValueTraversal : GetValueTraversal, GetValueStringTraversal
    {
        public string Value { get; set; }

        public GetStaticValueTraversal(string value)
            => Value = value;

        public string GetValue(string source)
        {
            return GetValue((object) source);
        }

        public string GetValue(object source)
        {
            if (string.IsNullOrWhiteSpace(Value))
                Process.ProcessObservable.GetInstance().Raise("GetStaticValueTraversal#1; Value is set to an empty string", "error");

            return Value;
        }
    }
}