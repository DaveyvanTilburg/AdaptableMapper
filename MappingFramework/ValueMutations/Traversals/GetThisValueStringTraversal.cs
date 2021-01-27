using MappingFramework.Configuration;

namespace MappingFramework.ValueMutations.Traversals
{
    public class GetThisValueStringTraversal : GetValueStringTraversal
    {
        public const string _typeId = "58473393-9844-4118-89ed-4295b90814b4";
        public string TypeId => _typeId;

        public GetThisValueStringTraversal() { }
        
        public string GetValue(Context context, string source)
            => source;
    }
}