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
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            XElement result = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\JsonSource_HardwareComposition.json"), System.IO.File.ReadAllText(@".\Resources\XmlTarget_HardwareTemplate.xml")) as XElement;

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_HardwareExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            errorObserver.GetRaisedWarnings().Count.Should().Be(0);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        private static MappingConfiguration GetMappingConfiguration()
        {
            var cpuCores = new Mapping(
                new Json.JsonGetValueTraversal(".CPU[0].Cores"),
                new Xml.XmlSetValue("./cpu/@cores")
            );

            var cpuSpeed = new Mapping(
                new Json.JsonGetValueTraversal(".CPU[0].Speed"),
                new Xml.XmlSetValue("./cpu/@speed")
            );

            var graphicalCardScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    cpuCores,
                    cpuSpeed
                },
                new Json.JsonGetScopeTraversal(".GraphicalCard[*]"),
                new Xml.XmlGetTemplate("./graphicalCard"),
                new Xml.XmlChildCreator()
            );

            var motherboardBrand = new Mapping(
                new Json.JsonGetValueTraversal(".Brand"),
                new Xml.XmlSetValue("./@motherboardBrand")
            );
            var motherboardCpuBrand = new Mapping(
                new Json.JsonGetValueTraversal(".CPU[0].Brand"),
                new Xml.XmlSetValue("./@cpuBrand")
            );
            var motherboardTotalStorage = new Mapping(
                new Json.JsonGetValueTraversal(".HardDrive[0].Size"),
                new Xml.XmlSetValue("./@storage")
            );
            var motherboardPartner = new Mapping(
                new Json.JsonGetSearchValueTraversal(
                    "../../../../../.Brand[?(@.Name=='{{searchValue}}')].Partner",
                    ".Brand"),
                new Xml.XmlSetValue("./@brandPartner")
            );

            var motherboardScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    graphicalCardScope
                },
                new List<Mapping>
                {
                    motherboardBrand,
                    motherboardCpuBrand,
                    motherboardTotalStorage,
                    motherboardPartner
                },
                new Json.JsonGetScopeTraversal("$.Computer.Motherboard[*]"),
                new Xml.XmlGetTemplate("//computers/computer"),
                new Xml.XmlChildCreator()
            );

            var memorySize = new Mapping(
                new Json.JsonGetValueTraversal("$.Size"),
                new Xml.XmlSetValue("./@size")
            );

            var memoryBrand = new Mapping(
                new Json.JsonGetValueTraversal("../../../.Brand"),
                new Xml.XmlSetValue("./@brand")
            );

            var memoryMotherboardBrand = new Mapping(
                new Json.JsonGetValueTraversal("../../../../../../.Brand"),
                new Xml.XmlSetValue("./@onMotherboardWithBrand")
            );

            var memoriesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memorySize,
                    memoryBrand,
                    memoryMotherboardBrand
                },
                new Json.JsonGetScopeTraversal("$.Computer.Motherboard[*].Memory[*].MemoryChip[*]"),
                new Xml.XmlGetTemplate("//allMemories/memory"),
                new Xml.XmlChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                memoriesScope,
                motherboardScope
            };

            var mappingConfiguration = new MappingConfiguration(
                scopes,
                new ContextFactory(
                    new Json.JsonObjectConverter(),
                    new Xml.XmlTargetInstantiator()
                ),
                new NullObjectConverter()
            );

            return mappingConfiguration;
        }
    }
}