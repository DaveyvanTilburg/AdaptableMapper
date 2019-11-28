using System.Collections.Generic;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class Tree
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