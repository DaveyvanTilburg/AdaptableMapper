using System.Collections.Generic;
using System.Linq;

namespace MappingFramework
{
    public class GenerateIdCache
    {
        private readonly List<AccessorLastGeneratedId> _accessorLastGenerateIds;

        public GenerateIdCache()
        {
            _accessorLastGenerateIds = new List<AccessorLastGeneratedId>();
        }

        public int GenerateNewId(object parent, string path, int startingNumber)
        {
            bool inCache = _accessorLastGenerateIds.Any(a => a.Parent == parent && a.Path == path);

            if (!inCache)
            {
                _accessorLastGenerateIds.Add(new AccessorLastGeneratedId
                {
                    Parent = parent,
                    Path = path,
                    LastGeneratedId = startingNumber
                });
            }

            AccessorLastGeneratedId accessorLastGeneratedId = _accessorLastGenerateIds.First(a => a.Parent == parent && a.Path == path);
            int result = accessorLastGeneratedId.LastGeneratedId;
            accessorLastGeneratedId.LastGeneratedId = result + 1;

            return result;
        }

        private class AccessorLastGeneratedId
        {
            public object Parent { get; set; }
            public string Path { get; set; }
            public int LastGeneratedId { get; set; }
        }
    }
}