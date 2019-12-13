namespace AdaptableMapper.Traversals
{
    public sealed class GenerateIdValueTraversal : GetValueTraversal
    {
        public int Number { get; set; }

        public string GetValue(object source)
        {
            string result = Number.ToString();
            Number++;

            return result;
        }
    }
}
