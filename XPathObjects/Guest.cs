using XPathSerialization;

namespace XPathObjects
{
    public class Guest : Adaptable
    {
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }
}