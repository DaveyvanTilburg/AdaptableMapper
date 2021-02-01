using System.Collections.Generic;
using MappingFramework.Configuration;

namespace MappingFramework.UnitTests.Cases.AdditionalSources
{
    public class TestAdditionalSource : AdditionalSource
    {
        string AdditionalSource.Name { get; } = "Mapping";

        IDictionary<string, string> AdditionalSource.GetValues()
        {
            return new Dictionary<string, string>
            {
                ["Davey-Profession"] = "Developer",
                ["Davey-DOB"] = "19910911",
                ["Joey-Profession"] = "LifeCoach",
                ["Joey-DOB"] = "19860906"
            };
        }
    }

    public class TestExtraAdditionalSource : AdditionalSource
    {
        string AdditionalSource.Name { get; } = "Values";

        IDictionary<string, string> AdditionalSource.GetValues()
        {
            return new Dictionary<string, string>
            {
                ["Identifier"] = "0123"
            };
        }
    }
}