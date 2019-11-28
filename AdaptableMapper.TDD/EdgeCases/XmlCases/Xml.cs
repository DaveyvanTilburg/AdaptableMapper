using System.Xml.Linq;

namespace AdaptableMapper.TDD.EdgeCases.XmlCases
{
    public class Xml
    {
        public static object CreateTarget(ContextType contextType)
        {
            object result = null;

            switch (contextType)
            {
                case ContextType.EmptyString:
                    result = string.Empty;
                    break;
                case ContextType.EmptyObject:
                    result = new XElement("nullObject");
                    break;
                case ContextType.TestObject:
                    result = CreateTestData();
                    break;
                case ContextType.InvalidType:
                    result = 0;
                    break;
                case ContextType.InvalidSource:
                    result = "abcd";
                    break;
            }

            return result;
        }

        private static XElement CreateTestData()
            => XElement.Parse(System.IO.File.ReadAllText("./Resources/Simple.xml"));
    }
}