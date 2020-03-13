using System.Collections.Generic;

namespace MappingFramework.Configuration
{
    public interface AdditionalSource
    {
        string Name { get; }

        IDictionary<string, string> GetValues();
    }
}