using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using MappingFramework.Configuration;
using Xunit;

namespace MappingFramework.TDD
{
    public class JsonToXml
    {
        [Fact]
        public void JsonToXmlTest()
        {
            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            MapResult mapResult = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\JsonSource_HardwareComposition.json"), System.IO.File.ReadAllText(@".\Resources\XmlTarget_HardwareTemplate.xml"));
            XElement result = mapResult.Result as XElement;

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_HardwareExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            mapResult.Information.Count.Should().Be(0);

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
                null,
                new Traversals.Json.JsonGetListValueTraversal(".GraphicalCard[*]"),
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
                new Compositions.GetSearchValueTraversal(
                    new Traversals.Json.JsonGetValueTraversal("../../../../../.Brand[?(@.Name=='{{searchValue}}')].Partner"),
                    new Traversals.Json.JsonGetValueTraversal(".Brand")
                ),
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
                null,
                new Traversals.Json.JsonGetListValueTraversal("$.Computer.Motherboard[*]"),
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
                null,
                new Traversals.Json.JsonGetListValueTraversal("$.Computer.Motherboard[*].Memory[*].MemoryChip[*]"),
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