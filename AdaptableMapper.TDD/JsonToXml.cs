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

            errorObserver.GetErrors().Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        private static MappingConfiguration GetMappingConfiguration()
        {
            var cpuCores = new Mapping(
                new Json.JsonGetValue(".CPU[0].Cores"),
                new Xml.XmlSetValue("./cpu/@cores")
            );

            var cpuSpeed = new Mapping(
                new Json.JsonGetValue(".CPU[0].Speed"),
                new Xml.XmlSetValue("./cpu/@speed")
            );

            var graphicalCardScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    cpuCores,
                    cpuSpeed
                },
                new Json.JsonGetScope(".GraphicalCard[*]"),
                new Xml.XmlTraversalThis(),
                new Xml.XmlTraversalTemplate("./graphicalCard"),
                new Xml.XmlChildCreator()
            );

            var motherboardBrand = new Mapping(
                new Json.JsonGetValue(".Brand"),
                new Xml.XmlSetValue("./@motherboardBrand")
            );
            var motherboardCpuBrand = new Mapping(
                new Json.JsonGetValue(".CPU[0].Brand"),
                new Xml.XmlSetValue("./@cpuBrand")
            );
            var motherboardTotalStorage = new Mapping(
                new Json.JsonGetValue(".HardDrive[0].Size"),
                new Xml.XmlSetValue("./@storage")
            );
            var motherboardPartner = new Mapping(
                new Json.JsonGetSearchValue(
                    "../../../../../.Brand[?(@.Name=='{{searchValue}}')].Partner",
                    ".Brand"),
                new Xml.XmlSetValue("./@brandPartner")
            );

            var motherboardScope = new MappingScopeComposite(
                new List<MappingScopeComposite>()
                {
                    graphicalCardScope
                },
                new List<Mapping>()
                {
                    motherboardBrand,
                    motherboardCpuBrand,
                    motherboardTotalStorage,
                    motherboardPartner
                },
                new Json.JsonGetScope("$.Computer.Motherboard[*]"),
                new Xml.XmlTraversal("//computers"),
                new Xml.XmlTraversalTemplate("./computer"),
                new Xml.XmlChildCreator()
            );

            var memorySize = new Mapping(
                new Json.JsonGetValue("$.Size"),
                new Xml.XmlSetValue("./@size")
            );

            var memoryBrand = new Mapping(
                new Json.JsonGetValue("../../../.Brand"),
                new Xml.XmlSetValue("./@brand")
            );

            var memoryMotherboardBrand = new Mapping(
                new Json.JsonGetValue("../../../../../../.Brand"),
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
                    memoriesScope,
                    motherboardScope
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