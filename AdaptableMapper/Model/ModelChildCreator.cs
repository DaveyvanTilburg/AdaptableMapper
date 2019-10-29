using AdaptableMapper.Memory.Language;
using System.Collections;

namespace AdaptableMapper.Memory
{
    public sealed class ModelChildCreator : ChildCreator
    {
        public object CreateChildOn(object parent, object template)
        {
            if (!(parent is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            if (!(template is IList parentProperty))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type IList");
                return string.Empty;
            }

            ModelBase newEntry = parentProperty.GetType().CreateAdaptable();
            newEntry.Parent = adaptable;
            parentProperty.Add(newEntry);

            return newEntry;
        }
    }
}