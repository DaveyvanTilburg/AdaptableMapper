using System.Collections;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.DataStructure.Configuration
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureChildCreator : ChildCreator, ResolvableByTypeId
    {
        public const string _typeId = "030fe21e-f4b9-4838-9aa0-960c3e8fa9a1";
        public string TypeId => _typeId;

        public DataStructureChildCreator() { }

        public object CreateChild(Context context, Template template)
            => ((IList)template.Child).GetType().CreateDataStructure(context);

        public void AddToParent(Context context, Template template, object newChild)
        {
            ((TraversableDataStructure)newChild).Parent = (TraversableDataStructure)template.Parent;
            ((IList)template.Child).Add(newChild);
        }
    }
}