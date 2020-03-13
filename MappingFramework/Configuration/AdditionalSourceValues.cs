using System.Collections.Generic;

namespace MappingFramework.Configuration
{
    public class AdditionalSourceValues
    {
        private readonly Dictionary<string, Dictionary<string, string>> _values = new Dictionary<string, Dictionary<string, string>>();

        public AdditionalSourceValues() { }

        public void AddAdditionalSource(AdditionalSource additionalSource)
        {
            if (_values.ContainsKey(additionalSource.Name))
            {
                Process.ProcessObservable.GetInstance().Raise($"AdditionalSourceValues#1; Duplicate name for AdditionalSource: {additionalSource.Name}", "error");
                return;
            }

            var value = new Dictionary<string, string>(additionalSource.GetValues());
            _values.Add(additionalSource.Name, value);
        }

        public string GetValue(string additionalSourceName, string key)
        {
            if (!_values.ContainsKey(additionalSourceName))
            {
                Process.ProcessObservable.GetInstance().Raise($"AdditionalSourceValues#2; No additionalSource defined with name {additionalSourceName}", "warning");
                return string.Empty;
            }

            if (!_values[additionalSourceName].ContainsKey(key))
            {
                Process.ProcessObservable.GetInstance().Raise($"AdditionalSourceValues#3; No value found for key {key} in additionalSource with name {additionalSourceName}", "warning");
                return string.Empty;
            }

            return _values[additionalSourceName][key];
        }
    }
}