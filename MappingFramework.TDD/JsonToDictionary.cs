using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Configuration.Dictionary;
using MappingFramework.Configuration.Json;
using MappingFramework.Dictionary;
using MappingFramework.Traversals.Dictionary;
using MappingFramework.Traversals.Json;
using Xunit;

namespace MappingFramework.TDD
{
    public class JsonToDictionary
    {
        [Fact]
        public void JsonToDictionaryFullStory()
        {
            var mappingConfiguration = new MappingConfiguration(
                new List<Mapping>
                {
                    new Mapping(
                        new JsonGetValueTraversal(".Computer.Motherboard[0].Brand"),
                        new DictionarySetValueTraversal("Brand")
                    ),
                    new Mapping(
                        new JsonGetValueTraversal(".Computer.Motherboard[0].CPU[0].Cores"),
                        new DictionarySetValueTraversal("CPUs", DictionaryValueTypes.Integer)
                    )
                },
                new ContextFactory(
                    new JsonObjectConverter(),
                    new DictionaryTargetInstantiator()
                ),
                new NullObjectConverter()
            );

            string source = File.ReadAllText("./Resources/JsonSource_HardwareComposition.json");
            var result = mappingConfiguration.Map(source, null) as EasyAccessDictionary;

            result.GetValueAs<string>("Brand").Should().Be("MSI");
            result.GetValueAs<int>("CPUs").Should().Be(2);
            result.GetValueAs<string>("Test").Should().Be(null);
        }
    }
}