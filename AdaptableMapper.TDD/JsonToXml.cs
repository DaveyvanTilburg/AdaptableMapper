using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Configuration;
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
                new Traversals.Json.JsonGetValueTraversal(".CPU[0].Cores"),
                new Traversals.Xml.XmlSetValueTraversal("./cpu/@cores")
            );

            var cpuSpeed = new Mapping(
                new Traversals.Json.JsonGetValueTraversal(".CPU[0].Speed"),
                new Traversals.Xml.XmlSetValueTraversal("./cpu/@speed")
            );

            var graphicalCardScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    cpuCores,
                    cpuSpeed
                },
                new Traversals.Json.JsonGetScopeTraversal(".GraphicalCard[*]"),
                new Traversals.Xml.XmlGetTemplateTraversal("./graphicalCard"),
                new Configuration.Xml.XmlChildCreator()
            );

            var motherboardBrand = new Mapping(
                new Traversals.Json.JsonGetValueTraversal(".Brand"),
                new Traversals.Xml.XmlSetValueTraversal("./@motherboardBrand")
            );
            var motherboardCpuBrand = new Mapping(
                new Traversals.Json.JsonGetValueTraversal(".CPU[0].Brand"),
                new Traversals.Xml.XmlSetValueTraversal("./@cpuBrand")
            );
            var motherboardTotalStorage = new Mapping(
                new Traversals.Json.JsonGetValueTraversal(".HardDrive[0].Size"),
                new Traversals.Xml.XmlSetValueTraversal("./@storage")
            );
            var motherboardPartner = new Mapping(
                new Traversals.Json.JsonGetSearchValueTraversal(
                    "../../../../../.Brand[?(@.Name=='{{searchValue}}')].Partner",
                    ".Brand"),
                new Traversals.Xml.XmlSetValueTraversal("./@brandPartner")
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
                new Traversals.Json.JsonGetScopeTraversal("$.Computer.Motherboard[*]"),
                new Traversals.Xml.XmlGetTemplateTraversal("//computers/computer"),
                new Configuration.Xml.XmlChildCreator()
            );

            var memorySize = new Mapping(
                new Traversals.Json.JsonGetValueTraversal("$.Size"),
                new Traversals.Xml.XmlSetValueTraversal("./@size")
            );

            var memoryBrand = new Mapping(
                new Traversals.Json.JsonGetValueTraversal("../../../.Brand"),
                new Traversals.Xml.XmlSetValueTraversal("./@brand")
            );

            var memoryMotherboardBrand = new Mapping(
                new Traversals.Json.JsonGetValueTraversal("../../../../../../.Brand"),
                new Traversals.Xml.XmlSetValueTraversal("./@onMotherboardWithBrand")
            );

            var memoriesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memorySize,
                    memoryBrand,
                    memoryMotherboardBrand
                },
                new Traversals.Json.JsonGetScopeTraversal("$.Computer.Motherboard[*].Memory[*].MemoryChip[*]"),
                new Traversals.Xml.XmlGetTemplateTraversal("//allMemories/memory"),
                new Configuration.Xml.XmlChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                memoriesScope,
                motherboardScope
            };

            var mappingConfiguration = new MappingConfiguration(
                scopes,
                new ContextFactory(
                    new Configuration.Json.JsonObjectConverter(),
                    new Configuration.Xml.XmlTargetInstantiator()
                ),
                new NullObjectConverter()
            );

            return mappingConfiguration;
        }
    }
}