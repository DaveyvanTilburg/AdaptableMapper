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
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return;
            }

            model.SetValue(PropertyName, value);
        }
    }
}