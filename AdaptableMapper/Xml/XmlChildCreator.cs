using System.Xml.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Xml
{
    public sealed class XmlChildCreator : ChildCreator
    {
        public object CreateChild(Template template)
        {
            if (!(template.Parent is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#10; parent is not of expected type XElement", "error", template.Parent?.GetType().Name);
                return new XElement("nullObject");
            }

            if (!(template.Child is XElement xTemplate))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#11; template is not of expected type XElement", "error", template.Child?.GetType().Name);
                return new XElement("nullObject");
            }

            var xTemplateCopy = new XElement(xTemplate);
            xElement.Add(xTemplateCopy);

            return xTemplateCopy;
        }
    }
}