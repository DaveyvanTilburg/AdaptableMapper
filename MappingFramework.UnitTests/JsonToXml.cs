using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.Languages.Json.Configuration;
using MappingFramework.Languages.Json.Traversals;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;
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
                new JsonGetValueTraversal(".CPU[0].Cores"),
                new XmlSetValueTraversal("./cpu/@cores")
            );

            var cpuSpeed = new Mapping(
                new JsonGetValueTraversal(".CPU[0].Speed"),
                new XmlSetValueTraversal("./cpu/@speed")
            );

            var graphicalCardScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    cpuCores,
                    cpuSpeed
                },
                new NullObject(),
                new JsonGetListValueTraversal(".GraphicalCard[*]"),
                new XmlGetTemplateTraversal("./graphicalCard"),
                new XmlChildCreator()
            );

            var motherboardBrand = new Mapping(
                new JsonGetValueTraversal(".Brand"),
                new XmlSetValueTraversal("./@motherboardBrand")
            );
            var motherboardCpuBrand = new Mapping(
                new JsonGetValueTraversal(".CPU[0].Brand"),
                new XmlSetValueTraversal("./@cpuBrand")
            );
            var motherboardTotalStorage = new Mapping(
                new JsonGetValueTraversal(".HardDrive[0].Size"),
                new XmlSetValueTraversal("./@storage")
            );
            var motherboardPartner = new Mapping(
                new Compositions.GetSearchValueTraversal(
                    new JsonGetValueTraversal("../../../../../.Brand[?(@.Name=='{{searchValue}}')].Partner"),
                    new JsonGetValueTraversal(".Brand")
                ),
                new XmlSetValueTraversal("./@brandPartner")
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
                new NullObject(),
                new JsonGetListValueTraversal("$.Computer.Motherboard[*]"),
                new XmlGetTemplateTraversal("//computers/computer"),
                new XmlChildCreator()
            );

            var memorySize = new Mapping(
                new JsonGetValueTraversal("$.Size"),
                new XmlSetValueTraversal("./@size")
            );

            var memoryBrand = new Mapping(
                new JsonGetValueTraversal("../../../.Brand"),
                new XmlSetValueTraversal("./@brand")
            );

            var memoryMotherboardBrand = new Mapping(
                new JsonGetValueTraversal("../../../../../../.Brand"),
                new XmlSetValueTraversal("./@onMotherboardWithBrand")
            );

            var memoriesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memorySize,
                    memoryBrand,
                    memoryMotherboardBrand
                },
                new NullObject(),
                new JsonGetListValueTraversal("$.Computer.Motherboard[*].Memory[*].MemoryChip[*]"),
                new XmlGetTemplateTraversal("//allMemories/memory"),
                new XmlChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                memoriesScope,
                motherboardScope
            };

            var mappingConfiguration = new MappingConfiguration(
                scopes,
                new ContextFactory(
                    new JsonSourceCreator(),
                    new XmlTargetCreator()
                ),
                new NullObject()
            );

            return mappingConfiguration;
        }
    }
}