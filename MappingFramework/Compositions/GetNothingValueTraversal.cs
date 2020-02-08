using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Compositions
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