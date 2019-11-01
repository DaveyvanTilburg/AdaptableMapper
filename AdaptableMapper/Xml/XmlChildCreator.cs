using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlChildCreator : ChildCreator
    {
        public object CreateChildOn(object parent, object template)
        {
            if (!(parent is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#10; parent is not of expected type XElement", "error", parent?.GetType().Name);
                return new XElement("nullObject");
            }

            if (!(template is XElement xTemplate))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#11; template is not of expected type XElement", "error", template?.GetType().Name);
                return new XElement("nullObject");
            }

            var xTemplateCopy = new XElement(xTemplate);
            xElement.Add(xTemplateCopy);

            return xTemplateCopy;
        }
    }
}