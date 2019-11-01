using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
{
    public sealed class ModelSetValueOnProperty : SetValueTraversal
    {
        public ModelSetValueOnProperty(string propertyName)
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