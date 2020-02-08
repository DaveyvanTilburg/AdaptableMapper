using System.IO;
using System.Linq;
using MappingFramework.Configuration.Json;
using MappingFramework.Configuration.Xml;
using MappingFramework.Traversals.Json;
using MappingFramework.Traversals.Xml;
using FluentAssertions;
using Xunit;

namespace MappingFramework.Builder.Tests
{
    public class BuilderTests
    {
        [Fact]
        public void Test()
        {
            string[] commands = File.ReadAllLines("./Resources/Mapping1.txt");

            var subject = new Builder();

            MappingConfiguration result = subject.Build(commands);
            result.Should().NotBeNull();
            result.ResultObjectConverter.Should().BeOfType(typeof(XElementToStringObjectConverter));

            result.ContextFactory.Should().NotBeNull();
            result.ContextFactory.ObjectConverter.Should().BeOfType(typeof(XmlObjectConverter));
            result.ContextFactory.TargetInstantiator.Should().BeOfType(typeof(JsonTargetInstantiator));

            result.Mappings.Count.Should().Be(1);
            result.Mappings.First().GetValueTraversal.Should().BeOfType(typeof(XmlGetValueTraversal));
            ((XmlGetValueTraversal) result.Mappings.First().GetValueTraversal).Path.Should().Be("count(./SimpleItems/SimpleItem)");

            result.Mappings.First().SetValueTraversal.Should().BeOfType(typeof(JsonSetValueTraversal));
            ((JsonSetValueTraversal)result.Mappings.First().SetValueTraversal).Path.Should().Be(".AmountOfSimpleItems");
        }
    }
}