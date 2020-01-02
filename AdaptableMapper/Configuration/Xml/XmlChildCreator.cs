using System.Xml.Linq;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Configuration.Xml
{
    public sealed class XmlChildCreator : ChildCreator, ResolvableByTypeId
    {
        public const string _typeId = "64df916a-8fef-46e1-9c34-6322365f6d22";
        public string TypeId => _typeId;

        public XmlChildCreator() { }

        public object CreateChild(Template template)
        {
            if (!(template.Parent is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XmlChildCreator#1; parent is not of expected type XElement", "error", template.Parent?.GetType().Name);
                return NullElement.Create();
            }

            if (!(template.Child is XElement xTemplate))
            {
                Process.ProcessObservable.GetInstance().Raise("XmlChildCreator#2; template is not of expected type XElement", "error", template.Child?.GetType().Name);
                return NullElement.Create();
            }

            var xTemplateCopy = new XElement(xTemplate);
            return xTemplateCopy;
        }

        public void AddToParent(Template template, object newChild)
        {
            if (!(template.Parent is XElement parent))
            {
                Process.ProcessObservable.GetInstance().Raise("XmlChildCreator#3; parent is not of expected type XElement", "error", template.Parent?.GetType().Name);
                return;
            }

            if (!(newChild is XElement xTemplate))
            {
                Process.ProcessObservable.GetInstance().Raise("XmlChildCreator#4; template is not of expected type XElement", "error", template.Child?.GetType().Name);
                return;
            }

            parent.Add(xTemplate);
        }
    }
}