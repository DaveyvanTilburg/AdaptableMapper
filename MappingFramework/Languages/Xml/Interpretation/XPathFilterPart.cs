namespace MappingFramework.Languages.Xml.Interpretation
{
    internal class XPathFilterPart : IXPathComponent
    {
        private readonly string _filter;
        private readonly IXPathComponent _next;

        public XPathFilterPart(string filter, IXPathComponent next)
        {
            _filter = filter;
            _next = next;
        }

        public string Compose()
            => $"*[local-name()='{_filter}']{_next.Compose()}";
    }
}