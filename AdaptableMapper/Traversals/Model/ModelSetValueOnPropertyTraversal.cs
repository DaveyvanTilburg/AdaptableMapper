using AdaptableMapper.Model;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelSetValueOnPropertyTraversal : SetValueTraversal
    {
        public ModelSetValueOnPropertyTraversal(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        public void SetValue(object target, string value)
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