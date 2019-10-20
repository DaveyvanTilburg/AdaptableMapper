using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XPathSerialization.XPathConfigurations
{
    internal partial class XPathMap : XPathConfiguration
    {
        public XPathMap(string xPath, string objectPath) : base(xPath, objectPath) { }

        public override void DeSerialize(XElement source, Adaptable target)
        {
            string elementValue = source.GetXPathValues(XPath).First();

            ObjectPath path = ObjectPath.CreateObjectPath(AdaptablePath);

            Adaptable pathTarget = target.GetOrCreateAdaptable(new Queue<string>(path.Path));
            pathTarget.SetValue(path.PropertyName, elementValue);
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            ObjectPath path = ObjectPath.CreateObjectPath(AdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(new Queue<string>(path.Path));
            string value = pathTarget.GetValue(path.PropertyName);

            target.SetXPathValues(XPath, value);
        }
    }
}