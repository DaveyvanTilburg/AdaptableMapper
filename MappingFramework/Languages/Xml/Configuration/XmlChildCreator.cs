using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Xml.Configuration
{
    [ContentType(ContentType.Xml)]
    public sealed class XmlChildCreator : ChildCreator, ResolvableByTypeId
    {
        public const string _typeId = "64df916a-8fef-46e1-9c34-6322365f6d22";
        public string TypeId => _typeId;

        public XmlChildCreator() { }

        public object CreateChild(Context context, Template template)
            => new XElement((XElement)template.Child);

        public void AddToParent(Context context, Template template, object newChild)
            => ((XElement)template.Parent).Add((XElement)newChild);
    }
}