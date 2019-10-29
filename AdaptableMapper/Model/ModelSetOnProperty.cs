using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public sealed class ModelSetOnProperty : SetValueTraversal
    {
        public ModelSetOnProperty(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return;
            }

            adaptable.SetValue(PropertyName, value);
        }
    }
}