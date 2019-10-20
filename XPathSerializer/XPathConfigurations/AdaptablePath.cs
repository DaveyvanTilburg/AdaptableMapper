using System.Collections.Generic;
using System.Linq;

namespace XPathSerialization.XPathConfigurations
{
    public class ObjectPath
    {
        public IReadOnlyList<string> Path { get; set; }
        public string PropertyName { get; set; }

        private ObjectPath(IReadOnlyList<string> path, string propertyName)
        {
            Path = path;
            PropertyName = propertyName;
        }

        public static ObjectPath CreateObjectPath(string objectPath)
        {
            var path = new Stack<string>(objectPath.Split('/'));
            string propertyName = path.Pop();

            return new ObjectPath(path.Reverse().ToList() as IReadOnlyList<string>, propertyName);
        }
    }
}