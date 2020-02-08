using Newtonsoft.Json.Linq;

namespace MappingFramework.TDD.Cases.JsonCases
{
    public class Json
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
                    result = new JObject();
                    break;
                case ContextType.TestObject:
                    result = CreateTestData();
                    break;
                case ContextType.InvalidObject:
                    result = new JArray();
                    break;
                case ContextType.InvalidType:
                    result = 0;
                    break;
                case ContextType.InvalidSource:
                    result = "abcd";
                    break;
                case ContextType.ValidParent:
                    result = CreateTestData().SelectToken("$.SimpleItems");
                    break;
                case ContextType.ValidSource:
                    result = System.IO.File.ReadAllText("./Resources/Simple.json");
                    break;
            }

            return result;
        }

        private static JToken CreateTestData()
            => JObject.Parse(System.IO.File.ReadAllText("./Resources/Simple.json"));
    }
}