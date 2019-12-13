using AdaptableMapper.Configuration;

namespace AdaptableMapper.Traversals
{
    public sealed class GenerateIdValueTraversal : GetValueTraversal
    {
        public int Number { get; set; }

        public string GetValue(Context context)
        {
            string result = Number.ToString();
            Number++;

            return result;
        }
    }
}