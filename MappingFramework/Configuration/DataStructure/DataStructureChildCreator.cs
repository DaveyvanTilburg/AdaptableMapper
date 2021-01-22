using System.Collections;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.DataStructure;
using MappingFramework.Traversals;

namespace MappingFramework.Configuration.DataStructure
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureChildCreator : ChildCreator, ResolvableByTypeId
    {
        public const string _typeId = "030fe21e-f4b9-4838-9aa0-960c3e8fa9a1";
        public string TypeId => _typeId;

        public DataStructureChildCreator() { }

        public object CreateChild(Template template)
        {
            if (!(template.Parent is TraversableDataStructure parent))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructureChildCreator#1; parent is not of expected type TraversableDataStructure", "error", template.Parent?.GetType().Name);
                return new NullDataStructure();
            }

            if (!(template.Child is IList parentProperty))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructureChildCreator#2; template is not of expected type IList", "error", template.Child?.GetType().Name);
                return new NullDataStructure();
            }

            TraversableDataStructure newEntry = parentProperty.GetType().CreateDataStructure();
            return newEntry;
        }

        public void AddToParent(Template template, object newChild)
        {
            if (!(template.Parent is TraversableDataStructure parent))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructureChildCreator#3; parent is not of expected type TraversableDataStructure", "error", template.Parent?.GetType().Name);
                return;
            }

            if (!(template.Child is IList parentProperty))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructureChildCreator#4; template is not of expected type IList", "error", template.Child?.GetType().Name);
                return;
            }

            if(!(newChild is TraversableDataStructure newChildInType))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructureChildCreator#5; template is not of expected type IList", "error", template.Child?.GetType().Name);
                return;
            }

            newChildInType.Parent = parent;
            parentProperty.Add(newChild);
        }
    }
}