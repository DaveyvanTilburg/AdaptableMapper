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
                    result = CreateTestData("./Resources/Simple.xml");
                    break;
                case ContextType.InvalidType:
                    result = 0;
                    break;
                case ContextType.InvalidSource:
                    result = "abcd";
                    break;
                case ContextType.AlternativeTestObject:
                    result = CreateTestData("./Resources/SimpleNamespace.xml");
                    break;
            }

            return result;
        }

        private static XElement CreateTestData(string path)
            => XElement.Parse(System.IO.File.ReadAllText(path));
    }
}