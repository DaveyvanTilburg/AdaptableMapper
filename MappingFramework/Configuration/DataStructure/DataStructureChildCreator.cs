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

        public object CreateChild(Context context, Template template)
        {
            if (!(template.Child is IList parentProperty))
            {
                context.InvalidType(template.Child, typeof(IList));
                return new NullDataStructure();
            }

            TraversableDataStructure newEntry = parentProperty.GetType().CreateDataStructure(context);
            return newEntry;
        }

        public void AddToParent(Context context, Template template, object newChild)
        {
            if (!(template.Parent is TraversableDataStructure parent))
            {
                context.InvalidType(template.Parent, typeof(TraversableDataStructure));
                return;
            }

            if (!(template.Child is IList parentProperty))
            {
                context.InvalidType(template.Child, typeof(IList));
                return;
            }

            ((TraversableDataStructure)newChild).Parent = parent;
            parentProperty.Add(newChild);
        }
    }
}