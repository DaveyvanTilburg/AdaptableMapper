using AdaptableMapper.Model;
using System.Collections;
using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Configuration.Model
{
    public sealed class ModelChildCreator : ChildCreator
    {
        public object CreateChild(Template template)
        {
            if (!(template.Parent is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#10; parent is not of expected type Model", "error", template.Parent?.GetType().Name);
                return new NullModel();
            }

            if (!(template.Child is IList parentProperty))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#11; template is not of expected type IList", "error", template.Child?.GetType().Name);
                return new NullModel();
            }

            ModelBase newEntry = parentProperty.GetType().CreateModel();
            newEntry.Parent = model;
            parentProperty.Add(newEntry);

            return newEntry;
        }
    }
}