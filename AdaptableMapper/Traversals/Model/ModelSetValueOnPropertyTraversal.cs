using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Model;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelSetValueOnPropertyTraversal : SetMutableValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "12151374-07cd-4a74-93e3-550e69ce61c0";
        public string TypeId => _typeId;

        public ModelSetValueOnPropertyTraversal() { }
        public ModelSetValueOnPropertyTraversal(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        protected override void SetValueImplementation(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#19; target is not of expected type Model", "error", PropertyName, context.Target);
                return;
            }

            model.SetValue(PropertyName, value);
        }
    }
}