using AdaptableMapper.Model.Language;
using System.Collections;

namespace AdaptableMapper.Model
{
    public sealed class ModelChildCreator : ChildCreator
    {
        public object CreateChildOn(object parent, object template)
        {
            if (!(parent is ModelBase model))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            if (!(template is IList parentProperty))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type IList");
                return string.Empty;
            }

            ModelBase newEntry = parentProperty.GetType().CreateModel();
            newEntry.Parent = model;
            parentProperty.Add(newEntry);

            return newEntry;
        }
    }
}