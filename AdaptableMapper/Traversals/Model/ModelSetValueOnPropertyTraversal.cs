using AdaptableMapper.Configuration;
using AdaptableMapper.Model;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelSetValueOnPropertyTraversal : SetMutableValueTraversal
    {
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