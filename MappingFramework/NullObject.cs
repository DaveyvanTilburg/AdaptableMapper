using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework
{
    public class NullObject : GetValueTraversal, SetValueTraversal, ValueMutation, Condition, GetValueStringTraversal
    {
        public const string _typeId = "8455065e-a596-413e-bb63-22cb0f34a87c";
        public string TypeId => _typeId;

        public string GetValue(Context context) => string.Empty;

        public void SetValue(Context context, MappingCaches mappingCaches, string value) { }

        public string Mutate(Context context, string value) => value;

        public bool Validate(Context context) => false;

        public string GetValue(string source) => string.Empty;
    }
}