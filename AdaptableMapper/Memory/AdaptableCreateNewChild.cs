using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;
using System.Collections;

namespace AdaptableMapper.Memory
{
    public sealed class AdaptableCreateNewChild : CreateNewChild
    {
        public object CreateChildOn(object parent, object template)
        {
            if (!(parent is Adaptable adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return string.Empty;
            }

            if (!(template is IList parentProperty))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type IList");
                return string.Empty;
            }

            Adaptable newEntry = parentProperty.GetType().CreateAdaptable();
            newEntry.Parent = adaptable;
            parentProperty.Add(newEntry);

            return newEntry;
        }
    }
}