using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Compositions
{
    public class GetNothingValueTraversal : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "52c6fd7d-8503-445c-a576-fe21bdaeba67";
        public string TypeId => _typeId;

        public GetNothingValueTraversal() { }

        public string GetValue(Context context)
        {
            return string.Empty;
        }
    }
}