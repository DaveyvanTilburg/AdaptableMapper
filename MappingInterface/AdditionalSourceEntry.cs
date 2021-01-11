using System.Collections.Generic;
using MappingFramework.Configuration;

namespace MappingFramework.MappingInterface
{
    internal class AdditionalSourceList : AdditionalSource
    {
        public string Name { get; set; }

        public List<AdditionalSourceEntry> Entries { get; set; }
        
        public IDictionary<string, string> GetValues()
        {
            var result = new Dictionary<string, string>();

            foreach (AdditionalSourceEntry entry in Entries)
                result.TryAdd(entry.Key, entry.Value);
            
            return result;
        }
    }
    
    public class AdditionalSourceEntry
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}