using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Dictionary;
using MappingFramework.Languages.Dictionary.Configuration;
using MappingFramework.Languages.Dictionary.Traversals;
using MappingFramework.Languages.Json.Configuration;
using MappingFramework.Languages.Json.Traversals;
using Xunit;

namespace MappingFramework.UnitTests
{
    public class JsonToDictionary
    {
        [Fact]
        public void JsonToDictionaryFullStory()
        {
            var mappingConfiguration = new MappingConfiguration(
                new List<Mapping>
                {
                    new(
                        new JsonGetValueTraversal(".Computer.Motherboard[0].Brand"),
                        new DictionarySetValueTraversal(new GetStaticValue("Brand"))
                    ),
                    new(
                        new JsonGetValueTraversal(".Computer.Motherboard[0].CPU[0].Cores"),
                        new DictionarySetValueTraversal(new GetStaticValue("CPUs"), DictionaryValueTypes.Integer)
                    )
                },
                new ContextFactory(
                    new JsonSourceCreator(),
                    new DictionaryTargetCreator()
                ),
                new NullObject()
            );

            string source = File.ReadAllText("./Resources/JsonSource_HardwareComposition.json");
            MapResult mapResult = mappingConfiguration.Map(source, null);
            var result = mapResult.Result as EasyAccessDictionary;

            result.GetValueAs<string>("Brand").Should().Be("MSI");
            result.GetValueAs<int>("CPUs").Should().Be(2);
            result.GetValueAs<string>("Test").Should().Be(null);
        }
    }
}