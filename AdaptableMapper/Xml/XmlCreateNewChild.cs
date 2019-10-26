using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public class XmlCreateNewChild : CreateNewChild
    {
        public object CreateChildOn(object parent, object template)
        {
            if(!(parent is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return new XElement("");
            }

            if (!(template is XElement xTemplate))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return new XElement("");
            }

            var xTemplateCopy = new XElement(xTemplate);
            xElement.Add(xTemplateCopy);

            return xTemplateCopy;
        }
    }
}