using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XPathSerialization.XPathConfigurations
{
    internal class XPathScope : XPathTransformation
    {
        public void Serialize(XPathConfiguration configuration, XElement source, Adaptable target)
        {
            IEnumerable<XElement> xScope = source.NavigateToPathSelectAll(configuration.XPath);

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(configuration.AdaptablePath);

            Adaptable pathTarget = target.GetOrCreateAdaptable(adaptablePathContainer.CreatePathQueue());
            IList adaptableScope = pathTarget.GetListProperty(adaptablePathContainer.PropertyName);

            foreach (XElement xScopeElement in xScope)
            {
                Adaptable newEntry = adaptableScope.GetType().CreateAdaptable();
                newEntry.SetParent(pathTarget);

                foreach (XPathConfiguration childConfiguration in configuration.Configurations)
                {
                    XPathTransformation childTransformation = XPathConfigurationRepository.GetInstance().GetConfiguration(childConfiguration.Type);
                    childTransformation.Serialize(childConfiguration, xScopeElement, newEntry);
                }

                adaptableScope.Add(newEntry);
            }
        }

        public void Deserialize(XPathConfiguration configuration, XElement target, Adaptable source)
        {
            XElement xTemplate = target.NavigateToPath(configuration.XPath);
            XElement xParent = xTemplate.GetParent();
            xTemplate.Remove();

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(configuration.AdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(adaptablePathContainer.CreatePathQueue());
            IList adaptableScope = pathTarget.GetListProperty(adaptablePathContainer.PropertyName);

            foreach (Adaptable sourceItem in adaptableScope)
            {
                var xTemplateCopy = new XElement(xTemplate);
                foreach (XPathConfiguration childConfiguration in configuration.Configurations)
                {
                    XPathTransformation childTransformation = XPathConfigurationRepository.GetInstance().GetConfiguration(childConfiguration.Type);
                    childTransformation.Deserialize(childConfiguration, xTemplateCopy, sourceItem);
                }

                xParent.Add(xTemplateCopy);
            }
        }
    }
}