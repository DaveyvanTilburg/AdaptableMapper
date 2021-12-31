namespace MappingFramework.Languages.Xml.Interpretation
{
    internal class XPathPart : IXPathComponent
    {
        private readonly string _expression;
        private readonly IXPathComponent _next;

        public XPathPart(string expression, IXPathComponent next)
        {
            _expression = expression;
            _next = next;
        }

        public string Compose()
            => $"{_expression}{_next.Compose()}";
    }
}