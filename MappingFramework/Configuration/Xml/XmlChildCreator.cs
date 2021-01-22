using System.Xml.Linq;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Configuration.Xml
{
    [ContentType(ContentType.Xml)]
    public sealed class XmlChildCreator : ChildCreator, ResolvableByTypeId
    {
        public const string _typeId = "64df916a-8fef-46e1-9c34-6322365f6d22";
        public string TypeId => _typeId;

        public XmlChildCreator() { }

        public object CreateChild(Template template)
            => new XElement((XElement)template.Child);

        public void AddToParent(Template template, object newChild)
            => ((XElement)template.Parent).Add((XElement)newChild);
    }
}