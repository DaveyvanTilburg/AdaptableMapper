using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper
{
    public class TemplateCache
    {
        public TemplateCache()
        {
            _cachedTemplates = new Dictionary<string, object>();
            _templateAccesses = new List<Access>();
        }

        private readonly Dictionary<string, object> _cachedTemplates;

        private readonly List<Access> _templateAccesses;

        public object GetTemplate(string name, object accessor)
        {
            _templateAccesses.Add(new Access(name, accessor));
            return _cachedTemplates.FirstOrDefault(k => k.Key.Equals(name)).Value;
        }

        public bool HasAccessed(string name, object accessor)
            => _templateAccesses.Any(a => a.Name.Equals(name) && a.Accessor.Equals(accessor));

        public void SetTemplate(string name, object template)
            => _cachedTemplates.Add(name, template);

        private class Access
        {
            public Access(string name, object accessor)
            {
                Name = name;
                Accessor = accessor;
            }

            public string Name { get; }
            public object Accessor { get; }
        }
    }
}