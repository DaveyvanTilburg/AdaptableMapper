using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class ConfigurationSaveAndLoad
    {
        [Fact]
        public void CheckIfSaveAndLoadMementoWorks()
        {
            MappingConfiguration source = XmlToModel.GetMappingConfiguration();

            string serialized = JsonSerializer.Serialize(source);
            var target = JsonSerializer.Deserialize<MappingConfiguration>(serialized);

            source.Should().BeEquivalentTo(target);
        }
    }
}