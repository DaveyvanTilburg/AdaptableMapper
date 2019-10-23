using System.Collections.Generic;

namespace XPathSerialization.XPathConfigurations
{
    public partial class XPathConfigurationRepository
    {
        private Dictionary<string, XPathTransformation> _configurations;

        private XPathConfigurationRepository()
        {
            _configurations = new Dictionary<string, XPathTransformation>();

            Register("Map", new XPathMap());
            Register("Scope", new XPathScope());
            Register("Search", new XPathSearch());
        }

        public void Register(string code, XPathTransformation xPathConfigurationBase)
        {
            _configurations.Add(code, xPathConfigurationBase);
        }

        public XPathTransformation GetConfiguration(string code)
        {
            if (!_configurations.TryGetValue(code, out XPathTransformation result))
                throw new XPathConfigurationException($"No configuration registered for code : {code}");

            return result;
        }
    }
}