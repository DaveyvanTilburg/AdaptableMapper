namespace AdaptableMapper.Traversals
{
    public class GetStaticValueTraversal : GetValueTraversal
    {
        public string Value { get; set; }

        public GetStaticValueTraversal(string value)
            => Value = value;

        public string GetValue(object source)
            => Value;
    }
}