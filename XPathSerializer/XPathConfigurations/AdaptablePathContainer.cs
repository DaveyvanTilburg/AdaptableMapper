using System.Collections.Generic;
using System.Linq;

namespace XPathSerialization.XPathConfigurations
{
    public class AdaptablePathContainer
    {
        private readonly IReadOnlyList<string> _path;
        public string PropertyName { get; }
        public Queue<string> GetPath()
        {
            return new Queue<string>(_path);
        }

        private AdaptablePathContainer(IReadOnlyList<string> path, string propertyName)
        {
            _path = path;
            PropertyName = propertyName;
        }

        public static AdaptablePathContainer CreateAdaptablePath(string adaptablePath)
        {
            Stack<string> pathStack = adaptablePath.ToStack();
            string propertyName = pathStack.Pop();

            var path = pathStack.Reverse().ToList();

            return new AdaptablePathContainer(path, propertyName);
        }
    }
}