using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework.Compositions
{
    [ContentType(ContentType.Any)]
    public class GetStaticValue : GetSearchPathValueTraversal, GetValueStringTraversal, ResolvableByTypeId
    {
        public const string _typeId = "136fe331-e3c2-496d-a7fc-e317b7eb80aa";
        public string TypeId => _typeId;

        public string Value { get; set; }
        string GetValueTraversalPathProperty.Path { get; set; } = string.Empty;
        
        public GetStaticValue() { }
        public GetStaticValue(string value)
            => Value = value;

        public string GetValue(Context context, string value)
        {
            return GetValue(context);
        }

        public string GetValue(Context context)
            => Value;
    }
}