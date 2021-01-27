using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Languages.DataStructure;
using MappingFramework.Languages.DataStructure.Configuration;
using MappingFramework.Languages.DataStructure.Traversals;
using MappingFramework.Languages.Json.Configuration;
using MappingFramework.Languages.Json.Traversals;
using MappingFramework.TDD.DataStructureExamples.Hardwares;
using Xunit;

namespace MappingFramework.TDD
{
    public class DataStructureToJson
    {
        [Fact]
        public void DataStructureToJsonTest()
        {
            MappingConfiguration mappingConfiguration = GetFakedMappingConfiguration();

            TraversableDataStructure source = CreateHardwareDataStructure();
            MapResult mapResult = mappingConfiguration.Map(source, System.IO.File.ReadAllText(@".\Resources\JsonTarget_HardwareTemplate.json"));
            JToken result = mapResult.Result as JToken;

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\JsonTarget_HardwareExpected.json");
            JToken jExpectedResult = JToken.Parse(expectedResult);

            mapResult.Information.Count.Should().Be(0);
            result.Should().BeEquivalentTo(jExpectedResult);
        }

        [Fact]
        public void DataStructureToJsonToString()
        {
            MappingConfiguration mappingConfiguration = GetFakedMappingConfiguration();
            mappingConfiguration.ResultObjectCreator = new JTokenToStringResultObjectCreator();

            TraversableDataStructure source = CreateHardwareDataStructure();
            MapResult mapResult = mappingConfiguration.Map(source, System.IO.File.ReadAllText(@".\Resources\JsonTarget_HardwareTemplate.json"));

            string result = mapResult.Result as string;
            result.Should().NotBeNull();

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\JsonTarget_HardwareExpected.txt");
            result.Should().BeEquivalentTo(expectedResult);
        }

        private static TraversableDataStructure CreateHardwareDataStructure()
        {
            var root = new Root();
            root.Motherboards.Add(CreateMotherboard1());
            root.Motherboards.Add(CreateMotherboard2());

            return root;
        }

        private static Motherboard CreateMotherboard1()
        {
            var harddrive1 = new HardDrive { Brand = "AData", Size = "524288", Speed = "Rotating" };

            var memoryChip5 = new MemoryChip { Size = "512" };
            var memoryChip6 = new MemoryChip { Size = "512" };
            var memory1 = new Memory { Type = "External" };
            memory1.MemoryChips.Add(memoryChip5);
            memory1.MemoryChips.Add(memoryChip6);

            var motherboard1cpu1 = new CPU
            {
                Brand = "Intel",
                Cores = "3",
                Speed = "Average"
            };

            var motherboard1graphicalcard1cpu1 = new CPU
            {
                Brand = "Intel",
                Cores = "4",
                Speed = "High"
            };
            var memoryChip1 = new MemoryChip { Size = "512" };
            var memoryChip2 = new MemoryChip { Size = "512" };
            var memoryChip3 = new MemoryChip { Size = "512" };
            var memoryChip4 = new MemoryChip { Size = "512" };
            var motherboard1graphicalcard1 = new GraphicalCard { Brand = "Nvidia" };
            motherboard1graphicalcard1.CPUs.Add(motherboard1graphicalcard1cpu1);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip1);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip2);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip3);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip4);

            var motherboard1 = new Motherboard { Brand = "MSI", CPU = motherboard1cpu1 };
            motherboard1.GraphicalCards.Add(motherboard1graphicalcard1);
            motherboard1.HardDrives.Add(harddrive1);
            motherboard1.Memories.Add(memory1);

            return motherboard1;
        }

        private static Motherboard CreateMotherboard2()
        {
            var harddrive1 = new HardDrive { Brand = "BData", Size = "262144", Speed = "Rotating" };

            var memoryChip5 = new MemoryChip { Size = "2048" };
            var memory1 = new Memory { Type = "Integrated" };
            memory1.MemoryChips.Add(memoryChip5);

            var memoryChip6 = new MemoryChip { Size = "2048" };
            var memory2 = new Memory { Type = "External" };
            memory2.MemoryChips.Add(memoryChip6);

            var motherboard1cpu1 = new CPU
            {
                Brand = "AMD",
                Cores = "1",
                Speed = "High"
            };

            var motherboard1graphicalcard1cpu1 = new CPU
            {
                Brand = "Intel",
                Cores = "6",
                Speed = "Medium"
            };
            var memoryChip1 = new MemoryChip { Size = "512" };
            var memoryChip2 = new MemoryChip { Size = "512" };
            var motherboard1graphicalcard1 = new GraphicalCard { Brand = "AMD" };
            motherboard1graphicalcard1.CPUs.Add(motherboard1graphicalcard1cpu1);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip1);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip2);

            var motherboard1 = new Motherboard { Brand = "Biostar", CPU = motherboard1cpu1 };
            motherboard1.GraphicalCards.Add(motherboard1graphicalcard1);
            motherboard1.HardDrives.Add(harddrive1);
            motherboard1.Memories.Add(memory1);
            motherboard1.Memories.Add(memory2);

            return motherboard1;
        }

        private static MappingConfiguration GetFakedMappingConfiguration()
        {
            var graphicalCardCpuBrand = new Mapping(
                new DataStructureGetValueTraversal("Brand"),
                new JsonSetValueTraversal(".Brand")
            );

            var graphicalCardCpuCores = new Mapping(
                new DataStructureGetValueTraversal("Cores"),
                new JsonSetValueTraversal(".Cores")
            );

            var graphicalCardCpuSpeed = new Mapping(
                new DataStructureGetValueTraversal("Speed"),
                new JsonSetValueTraversal(".Speed")
            );

            var graphicalCardCpuScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    graphicalCardCpuBrand,
                    graphicalCardCpuCores,
                    graphicalCardCpuSpeed
                },
                new NullObject(),
                new DataStructureGetListValueTraversal("CPUs"),
                new JsonGetTemplateTraversal("$.CPUs[0]"),
                new JsonChildCreator()
            );

            var graphicalCardMemoryChipSize = new Mapping(
                new DataStructureGetValueTraversal("Size"),
                new JsonSetValueTraversal(".Size")
            );

            var graphicalCardMemoryChipScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    graphicalCardMemoryChipSize
                },
                new NullObject(),
                new DataStructureGetListValueTraversal("MemoryChips"),
                new JsonGetTemplateTraversal(".MemoryChips[0]"),
                new JsonChildCreator()
            );

            var graphicalCardBrand = new Mapping(
                new DataStructureGetValueTraversal("Brand"),
                new JsonSetValueTraversal(".Brand")
            );

            var graphicalCardsScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    graphicalCardMemoryChipScope,
                    graphicalCardCpuScope
                },
                new List<Mapping>
                {
                    graphicalCardBrand
                },
                new NullObject(),
                new DataStructureGetListValueTraversal("GraphicalCards"),
                new JsonGetTemplateTraversal(".GraphicalCards[0]"),
                new JsonChildCreator()
            );

            var motherboardMemorySize = new Mapping(
                new DataStructureGetValueTraversal("Size"),
                new JsonSetValueTraversal(".Size")
            );

            var memoryChipsScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    motherboardMemorySize
                },
                new NullObject(),
                new DataStructureGetListValueTraversal("Memories{'PropertyName':'Type','Value':'External'}/MemoryChips"),
                new JsonGetTemplateTraversal(".CPUs[0].MemoryChips[0]"),
                new JsonChildCreator()
            );

            var motherBoardCpuBrand = new Mapping(
                new DataStructureGetValueTraversal("CPU/Brand"),
                new JsonSetValueTraversal(".CPUs[0].Brand")
            );

            var motherBoardCpuCores = new Mapping(
                new DataStructureGetValueTraversal("CPU/Cores"),
                new JsonSetValueTraversal(".CPUs[0].Cores")
            );

            var motherBoardCpuSpeed = new Mapping(
                new DataStructureGetValueTraversal("CPU/Speed"),
                new JsonSetValueTraversal(".CPUs[0].Speed")
            );

            var motherboardBrand = new Mapping(
                new DataStructureGetValueTraversal("Brand"),
                new JsonSetValueTraversal(".Brand")
            );

            var motherboardScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    graphicalCardsScope,
                    memoryChipsScope
                },
                new List<Mapping>
                {
                    motherboardBrand,
                    motherBoardCpuBrand,
                    motherBoardCpuCores,
                    motherBoardCpuSpeed
                },
                new NullObject(),
                new DataStructureGetListValueTraversal("Motherboards"),
                new JsonGetTemplateTraversal("$.Motherboards[0]"),
                new JsonChildCreator()
            );

            var hardDriveBrand = new Mapping(
                new DataStructureGetValueTraversal("Brand"),
                new JsonSetValueTraversal(".Brand")
            );

            var hardDriveSize = new Mapping(
                new DataStructureGetValueTraversal("Size"),
                new JsonSetValueTraversal(".Size")
            );

            var hardDriveSpeed = new Mapping(
                new DataStructureGetValueTraversal("Speed"),
                new JsonSetValueTraversal(".Speed")
            );

            var hardDrivesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    hardDriveBrand,
                    hardDriveSize,
                    hardDriveSpeed
                },
                new NullObject(),
                new DataStructureGetListValueTraversal("Motherboards/HardDrives"),
                new JsonGetTemplateTraversal("$.AvailableHardDrives[0]"),
                new JsonChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                hardDrivesScope,
                motherboardScope
            };

            var contextFactory = new ContextFactory(
                new DataStructureSourceCreator(),
                new JsonTargetCreator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObject());

            return mappingConfiguration;
        }
    }
}