using System.Collections.Generic;
using MappingFramework.Configuration;
using Xunit;

namespace MappingFramework.UnitTests.Cases
{
    public class Configuration
    {
        [Fact]
        public void MappingConfigurationMappingConstructor()
        {
            var subject = new MappingConfiguration(new List<Mapping>(), null, null);
        }

        [Fact]
        public void MappingConfigurationMappingAndScopesConstructor()
        {
            var subject = new MappingConfiguration(new List<MappingScopeComposite>(), new List<Mapping>(), null, null);
        }
    }
}