using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public sealed class AdaptableSetOnProperty : SetValueTraversal
    {
        public AdaptableSetOnProperty(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is Adaptable adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return;
            }

            adaptable.SetValue(PropertyName, value);
        }
    }
}