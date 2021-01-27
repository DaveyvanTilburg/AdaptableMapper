using System.Collections.Generic;

namespace MappingFramework.Configuration
{
    public class AdditionalSourceValues
    {
        private readonly Dictionary<string, Dictionary<string, string>> _values = new Dictionary<string, Dictionary<string, string>>();

        public AdditionalSourceValues() { }

        public void AddAdditionalSource(AdditionalSource additionalSource, Context context)
        {
            Dictionary<string, string> dictionary;
            if (_values.ContainsKey(additionalSource.Name))
                dictionary = _values[additionalSource.Name];
            else
            {
                dictionary = new Dictionary<string, string>();
                _values.Add(additionalSource.Name, dictionary);
            }

            foreach (KeyValuePair<string, string> kvp in additionalSource.GetValues())
                if (!dictionary.ContainsKey(kvp.Key))
                    dictionary.Add(kvp.Key, kvp.Value);
                else
                    context.AddInformation($"Duplicate key for additional source: {kvp.Key}", InformationType.Error);
        }

        public string GetValue(string additionalSourceName, string key, Context context)
        {
            if (!_values.ContainsKey(additionalSourceName))
            {
                context.AddInformation($"No additionalSource defined with name {additionalSourceName}", InformationType.Warning);
                return string.Empty;
            }

            if (!_values[additionalSourceName].ContainsKey(key))
            {
                context.AddInformation($"No additionalSource value defined with key {key}", InformationType.Warning);
                return string.Empty;
            }

            return _values[additionalSourceName][key];
        }
    }
}