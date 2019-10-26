using System.Xml.Linq;

namespace XPathSerialization.Traversals.XmlTraversals
{
    public class XmlCreateNewChild : CreateNewChild
    {
        private XElement _parent;

        protected override object DuplicateTemplate(object template)
        {
            if (!(template is XElement xTemplate))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return new XElement("");
            }

            var xTemplateCopy = new XElement(xTemplate);
            _parent.Add(xTemplateCopy);

            return xTemplateCopy;
        }

        protected override object GetTemplate(object target)
        {
            if (!(target is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return new XElement("");
            }

            _parent = xElement.GetParent();
            xElement.Remove();

            return xElement;
        }
    }
}