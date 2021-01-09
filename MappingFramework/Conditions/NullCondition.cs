using MappingFramework.Configuration;

namespace MappingFramework.Conditions
{
    public class NullCondition : Condition
    {
        public const string _typeId = "c977b67b-3c4e-4765-bedc-1e08a360f35f";
        public string TypeId => _typeId;

        public NullCondition() { }

        public bool Validate(Context context) => false;
    }
}