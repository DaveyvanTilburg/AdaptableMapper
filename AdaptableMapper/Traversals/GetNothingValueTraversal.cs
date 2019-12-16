using AdaptableMapper.Configuration;

namespace AdaptableMapper.Traversals
{
    public class GetNothingValueTraversal : GetValueTraversal
    {
        public string GetValue(Context context)
        {
            return string.Empty;
        }
    }
}