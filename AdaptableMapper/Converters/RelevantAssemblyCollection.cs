using System.Collections.Generic;

namespace AdaptableMapper.Converters
{
    public static class RelevantAssemblyCollection
    {
        private static readonly List<string> _assemblyNames;

        static RelevantAssemblyCollection()
            => _assemblyNames = new List<string> { "AdaptableMapper" };

        public static void AddAssemblyName(string name) => _assemblyNames.Add(name);

        public static IReadOnlyCollection<string> GetAssemblyNames() => _assemblyNames;
    }
}