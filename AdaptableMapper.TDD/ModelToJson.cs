using AdaptableMapper.Model;
using FluentAssertions;
using ModelObjects.Hardwares;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using AdaptableMapper.Configuration;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class ModelToJson
    {
        [Fact]
        public void ModelToJsonTest()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetFakedMappingConfiguration();

            ModelBase source = CreateHardwareModel();
            JToken result = mappingConfiguration.Map(source, System.IO.File.ReadAllText(@".\Resources\JsonTarget_HardwareTemplate.json")) as JToken;

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\JsonTarget_HardwareExpected.json");
            JToken jExpectedResult = JToken.Parse(expectedResult);

            errorObserver.GetRaisedWarnings().Count.Should().Be(0);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Should().BeEquivalentTo(jExpectedResult);
        }

        [Fact]
        public void ModelToJsonToString()
        {
            MappingConfiguration mappingConfiguration = GetFakedMappingConfiguration();
            mappingConfiguration.ResultObjectConverter = new Configuration.Json.JTokenToStringObjectConverter();

            ModelBase source = CreateHardwareModel();
            object resultObject = mappingConfiguration.Map(source, System.IO.File.ReadAllText(@".\Resources\JsonTarget_HardwareTemplate.json"));

            var result = resultObject as string;
            result.Should().NotBeNull();

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\JsonTarget_HardwareExpected.txt");
            result.Should().BeEquivalentTo(expectedResult);
        }

        private static ModelBase CreateHardwareModel()
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
                new Traversals.Model.ModelGetValueTraversal("Brand"),
                new Traversals.Json.JsonSetValueTraversal(".Brand")
            );

            var graphicalCardCpuCores = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Cores"),
                new Traversals.Json.JsonSetValueTraversal(".Cores")
            );

            var graphicalCardCpuSpeed = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Speed"),
                new Traversals.Json.JsonSetValueTraversal(".Speed")
            );

            var graphicalCardCpuScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    graphicalCardCpuBrand,
                    graphicalCardCpuCores,
                    graphicalCardCpuSpeed
                },
                new Traversals.Model.ModelGetScopeTraversal("CPUs"),
                new Traversals.Json.JsonGetTemplateTraversal("$.CPUs[0]"),
                new Configuration.Json.JsonChildCreator()
            );

            var graphicalCardMemoryChipSize = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Size"),
                new Traversals.Json.JsonSetValueTraversal(".Size")
            );

            var graphicalCardMemoryChipScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    graphicalCardMemoryChipSize
                },
                new Traversals.Model.ModelGetScopeTraversal("MemoryChips"),
                new Traversals.Json.JsonGetTemplateTraversal(".MemoryChips[0]"),
                new Configuration.Json.JsonChildCreator()
            );

            var graphicalCardBrand = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Brand"),
                new Traversals.Json.JsonSetValueTraversal(".Brand")
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
                new Traversals.Model.ModelGetScopeTraversal("GraphicalCards"),
                new Traversals.Json.JsonGetTemplateTraversal(".GraphicalCards[0]"),
                new Configuration.Json.JsonChildCreator()
            );

            var motherboardMemorySize = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Size"),
                new Traversals.Json.JsonSetValueTraversal(".Size")
            );

            var memoryChipsScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    motherboardMemorySize
                },
                new Traversals.Model.ModelGetScopeTraversal("Memories{'PropertyName':'Type','Value':'External'}/MemoryChips"),
                new Traversals.Json.JsonGetTemplateTraversal(".CPUs[0].MemoryChips[0]"),
                new Configuration.Json.JsonChildCreator()
            );

            var motherBoardCpuBrand = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("CPU/Brand"),
                new Traversals.Json.JsonSetValueTraversal(".CPUs[0].Brand")
            );

            var motherBoardCpuCores = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("CPU/Cores"),
                new Traversals.Json.JsonSetValueTraversal(".CPUs[0].Cores")
            );

            var motherBoardCpuSpeed = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("CPU/Speed"),
                new Traversals.Json.JsonSetValueTraversal(".CPUs[0].Speed")
            );

            var motherboardBrand = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Brand"),
                new Traversals.Json.JsonSetValueTraversal(".Brand")
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
                new Traversals.Model.ModelGetScopeTraversal("Motherboards"),
                new Traversals.Json.JsonGetTemplateTraversal("$.Motherboards[0]"),
                new Configuration.Json.JsonChildCreator()
            );

            var hardDriveBrand = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Brand"),
                new Traversals.Json.JsonSetValueTraversal(".Brand")
            );

            var hardDriveSize = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Size"),
                new Traversals.Json.JsonSetValueTraversal(".Size")
            );

            var hardDriveSpeed = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Speed"),
                new Traversals.Json.JsonSetValueTraversal(".Speed")
            );

            var hardDrivesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    hardDriveBrand,
                    hardDriveSize,
                    hardDriveSpeed
                },
                new Traversals.Model.ModelGetScopeTraversal("Motherboards/HardDrives"),
                new Traversals.Json.JsonGetTemplateTraversal("$.AvailableHardDrives[0]"),
                new Configuration.Json.JsonChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                hardDrivesScope,
                motherboardScope
            };

            var contextFactory = new ContextFactory(
                new Configuration.Model.ModelObjectConverter(),
                new Configuration.Json.JsonTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}