using System.Collections.Generic;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework
{
    [ContentType(ContentType.Any)]
    public class NullObject : GetSearchPathValueTraversal, GetListSearchPathValueTraversal, SetValueTraversal, ValueMutation, Condition, GetValueStringTraversal, ResultObjectConverter
    {
        public const string _typeId = "8455065e-a596-413e-bb63-22cb0f34a87c";
        public string TypeId => _typeId;

        public string GetValue(Context context) => string.Empty;

        public void SetValue(Context context, MappingCaches mappingCaches, string value) { }

        public string Mutate(Context context, string value) => value;

        public bool Validate(Context context) => true;

        public string GetValue(Context context, string source) => source;
        public object Convert(object source) => source;
        public string Path { get; set; } = string.Empty;
        public MethodResult<IEnumerable<object>> GetValues(Context context) => new MethodResult<IEnumerable<object>>(new List<object>());
    }
}