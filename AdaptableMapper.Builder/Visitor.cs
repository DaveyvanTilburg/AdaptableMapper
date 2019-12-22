using System.Collections.Generic;

namespace AdaptableMapper.Builder
{
    internal class Visitor
    {
        public Visitor()
        {
            Result = new MappingConfiguration();
            _stashedObjects = new Dictionary<string, object>();
        }

        public MappingConfiguration Result { get; }
        public object Subject { get; set; }
        public Command Command { get; set; }

        private readonly Dictionary<string, object> _stashedObjects;

        public void Stash(string name, object item)
            => _stashedObjects[name] = item;

        public object DeStash(string name)
        {
            var result = _stashedObjects[name];
            _stashedObjects.Remove(name);

            return result;
        }
    }
}