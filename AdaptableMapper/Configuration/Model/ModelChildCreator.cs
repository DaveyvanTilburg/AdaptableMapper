using AdaptableMapper.Model;
using System.Collections;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Configuration.Model
{
    public sealed class ModelChildCreator : ChildCreator, SerializableByTypeId
    {
        public const string _typeId = "030fe21e-f4b9-4838-9aa0-960c3e8fa9a1";
        public string TypeId => _typeId;

        public ModelChildCreator() { }

        public object CreateChild(Template template)
        {
            if (!(template.Parent is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("ModelChildCreator#1; parent is not of expected type Model", "error", template.Parent?.GetType().Name);
                return new NullModel();
            }

            if (!(template.Child is IList parentProperty))
            {
                Process.ProcessObservable.GetInstance().Raise("ModelChildCreator#2; template is not of expected type IList", "error", template.Child?.GetType().Name);
                return new NullModel();
            }

            ModelBase newEntry = parentProperty.GetType().CreateModel();
            return newEntry;
        }

        public void AddToParent(Template template, object newChild)
        {
            if (!(template.Parent is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("ModelChildCreator#3; parent is not of expected type Model", "error", template.Parent?.GetType().Name);
                return;
            }

            if (!(template.Child is IList parentProperty))
            {
                Process.ProcessObservable.GetInstance().Raise("ModelChildCreator#4; template is not of expected type IList", "error", template.Child?.GetType().Name);
                return;
            }

            if(!(newChild is ModelBase newChildModel))
            {
                Process.ProcessObservable.GetInstance().Raise("ModelChildCreator#5; template is not of expected type IList", "error", template.Child?.GetType().Name);
                return;
            }

            newChildModel.Parent = model;
            parentProperty.Add(newChildModel);
        }
    }
}