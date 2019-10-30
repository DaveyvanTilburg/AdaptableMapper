using AdaptableMapper.Contexts;
using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class JsonToXml
    {
        [Fact]
        public void JsonToXmlTest()
        {
            var errorObserver = new TestErrorObserver();
            Errors.ErrorObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            XElement result = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\JsonSource_HardwareComposition.json")) as XElement;

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_HardwareExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            //errorObserver.GetErrors().Count.Should().Be(2);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        private static MappingConfiguration GetMappingConfiguration()
        {
            var memorySize = new Mapping(
                new Json.JsonGetValue("$.Size"),
                new Xml.XmlSetValue("./@size")
            );

            var memoryBrand = new Mapping(
                new Json.JsonGetValue("$..Memory.Brand"),
                new Xml.XmlSetValue("./@brand")
            );

            var memoryMotherboardBrand = new Mapping(
                new Json.JsonGetValue("$..Motherboard.Brand"),
                new Xml.XmlSetValue("./@onMotherboardWithBrand")
            );

            var memoriesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    memorySize,
                    memoryBrand,
                    memoryMotherboardBrand
                },
                new Json.JsonGetScope("$.Computer.Motherboard[*].Memory[*].MemoryChip[*]"),
                new Xml.XmlTraversal("//allMemories"),
                new Xml.XmlTraversalTemplate("./memory"),
                new Xml.XmlChildCreator()
            );

            var mappingScopeRoot = new MappingScopeRoot(
                new List<MappingScopeComposite>()
                {
                    memoriesScope
                }
            );

            var mappingConfiguration = new MappingConfiguration(
                mappingScopeRoot,
                new ContextFactory(
                    new Json.JsonObjectConverter(),
                    new Xml.XmlTargetInstantiator(System.IO.File.ReadAllText(@".\Resources\XmlTarget_HardwareTemplate.xml"))
                ),
                new NullObjectConverter()
            );

            return mappingConfiguration;
        }
    }
}