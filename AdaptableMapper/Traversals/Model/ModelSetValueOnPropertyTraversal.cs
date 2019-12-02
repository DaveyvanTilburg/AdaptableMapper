using AdaptableMapper.Model;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelSetValueOnPropertyTraversal : SetFormattedValueTraversal
    {
        public ModelSetValueOnPropertyTraversal(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        protected override void SetValueImplementation(object target, string value)
        {
            if (!(target is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#19; target is not of expected type Model", "error", PropertyName, target);
                return;
            }

            model.SetValue(PropertyName, value);
        }
    }
}