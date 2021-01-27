using System;
using System.Collections.Generic;

namespace MappingFramework.Caches
{
    public class MappingCaches
    {
        private readonly Dictionary<string, object> _storedObjects;

        public MappingCaches()
        {
            _storedObjects = new Dictionary<string, object>();
        }

        public T GetCache<T>(string cacheName)
        {
            if (!_storedObjects.ContainsKey(cacheName))
                _storedObjects.Add(cacheName, Activator.CreateInstance<T>());

            return (T)_storedObjects[cacheName];
        }
    }
}