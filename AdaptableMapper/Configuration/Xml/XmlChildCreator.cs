using System.Xml.Linq;
using AdaptableMapper.Traversals;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Configuration.Xml
{
    public sealed class XmlChildCreator : ChildCreator
    {
        public object CreateChild(Template template)
        {
            if (!(template.Parent is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#10; parent is not of expected type XElement", "error", template.Parent?.GetType().Name);
                return NullElement.Create();
            }

            if (!(template.Child is XElement xTemplate))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#11; template is not of expected type XElement", "error", template.Child?.GetType().Name);
                return NullElement.Create();
            }

            var xTemplateCopy = new XElement(xTemplate);
            xElement.Add(xTemplateCopy);

            return xTemplateCopy;
        }
    }
}