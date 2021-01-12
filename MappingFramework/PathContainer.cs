using System.Collections.Generic;
using System.Linq;

namespace MappingFramework
{
    internal class PathContainer
    {
        private readonly IReadOnlyList<string> _path;
        public string LastInPath { get; }
        public Queue<string> CreatePathQueue()
        {
            return new Queue<string>(_path);
        }

        private PathContainer(IReadOnlyList<string> path, string lastInPath)
        {
            _path = path;
            LastInPath = lastInPath;
        }

        public static PathContainer Create(string dataStructurePath)
        {
            Stack<string> pathStack = dataStructurePath.ToStack();
            string lastInPath = pathStack.Pop();

            var path = pathStack.Reverse().ToList();
            return new PathContainer(path, lastInPath);
        }
    }
}