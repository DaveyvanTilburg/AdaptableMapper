namespace AdaptableMapper.XPathConfigurations
{
    public partial class XPathConfigurationRepository
    {
        private static XPathConfigurationRepository _instance;
        public static XPathConfigurationRepository GetInstance()
        {
            if (_instance == null)
                _instance = new XPathConfigurationRepository();

            return _instance;
        }
    }
}