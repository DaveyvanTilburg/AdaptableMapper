using AdaptableMapper.Model.Language;
using FluentAssertions;
using ModelObjects.Hardwares;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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
            JToken result = mappingConfiguration.Map(source) as JToken;

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
            mappingConfiguration.ObjectConverter = new Json.JTokenToStringObjectConverter();

            ModelBase source = CreateHardwareModel();
            object resultObject = mappingConfiguration.Map(source);

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
            var memory1 = new Memory();
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
            var memory1 = new Memory();
            memory1.MemoryChips.Add(memoryChip5);

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

            return motherboard1;
        }

        private static MappingConfiguration GetFakedMappingConfiguration()
        {
            var graphicalCardCpuBrand = new Mapping(
                new Model.ModelGetValue("Brand"),
                new Json.JsonSetValue(".Brand")
            );

            var graphicalCardCpuCores = new Mapping(
                new Model.ModelGetValue("Cores"),
                new Json.JsonSetValue(".Cores")
            );

            var graphicalCardCpuSpeed = new Mapping(
                new Model.ModelGetValue("Speed"),
                new Json.JsonSetValue(".Speed")
            );

            var graphicalCardCpuScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    graphicalCardCpuBrand,
                    graphicalCardCpuCores,
                    graphicalCardCpuSpeed
                },
                new Model.ModelGetScope("CPUs"),
                new Json.JsonTraversal("$.CPUs"),
                new Json.JsonTraversalTemplate("[0]"),
                new Json.JsonChildCreator()
            );

            var graphicalCardMemoryChipSize = new Mapping(
                new Model.ModelGetValue("Size"),
                new Json.JsonSetValue(".Size")
            );

            var graphicalCardMemoryChipScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    graphicalCardMemoryChipSize
                },
                new Model.ModelGetScope("MemoryChips"),
                new Json.JsonTraversal(".MemoryChips"),
                new Json.JsonTraversalTemplate("[0]"),
                new Json.JsonChildCreator()
            );

            var graphicalCardBrand = new Mapping(
                new Model.ModelGetValue("Brand"),
                new Json.JsonSetValue(".Brand")
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
                new Model.ModelGetScope("GraphicalCards"),
                new Json.JsonTraversal(".GraphicalCards"),
                new Json.JsonTraversalTemplate("[0]"),
                new Json.JsonChildCreator()
            );

            var motherboardMemorySize = new Mapping(
                new Model.ModelGetValue("Size"),
                new Json.JsonSetValue(".Size")
            );

            var memoryChipsScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    motherboardMemorySize
                },
                new Model.ModelGetScope("Memories/MemoryChips"),
                new Json.JsonTraversal(".CPUs[0].MemoryChips"),
                new Json.JsonTraversalTemplate("[0]"),
                new Json.JsonChildCreator()
            );

            var motherBoardCpuBrand = new Mapping(
                new Model.ModelGetValue("CPU/Brand"),
                new Json.JsonSetValue(".CPUs[0].Brand")
            );

            var motherBoardCpuCores = new Mapping(
                new Model.ModelGetValue("CPU/Cores"),
                new Json.JsonSetValue(".CPUs[0].Cores")
            );

            var motherBoardCpuSpeed = new Mapping(
                new Model.ModelGetValue("CPU/Speed"),
                new Json.JsonSetValue(".CPUs[0].Speed")
            );

            var motherboardBrand = new Mapping(
                new Model.ModelGetValue("Brand"),
                new Json.JsonSetValue(".Brand")
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
                new Model.ModelGetScope("Motherboards"),
                new Json.JsonTraversal("$.Motherboards"),
                new Json.JsonTraversalTemplate("[0]"),
                new Json.JsonChildCreator()
            );

            var hardDriveBrand = new Mapping(
                new Model.ModelGetValue("Brand"),
                new Json.JsonSetValue(".Brand")
            );

            var hardDriveSize = new Mapping(
                new Model.ModelGetValue("Size"),
                new Json.JsonSetValue(".Size")
            );

            var hardDriveSpeed = new Mapping(
                new Model.ModelGetValue("Speed"),
                new Json.JsonSetValue(".Speed")
            );

            var hardDrivesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    hardDriveBrand,
                    hardDriveSize,
                    hardDriveSpeed
                },
                new Model.ModelGetScope("Motherboards/HardDrives"),
                new Json.JsonTraversal("$.AvailableHardDrives"),
                new Json.JsonTraversalTemplate("[0]"),
                new Json.JsonChildCreator()
            );

            var rootScope = new MappingScopeRoot(
                new List<MappingScopeComposite>
                {
                    hardDrivesScope,
                    motherboardScope
                }
            );

            var contextFactory = new Contexts.ContextFactory(
                new Model.ModelObjectConverter(),
                new Json.JsonTargetInstantiator(System.IO.File.ReadAllText(@".\Resources\JsonTarget_HardwareTemplate.json"))
            );

            var mappingConfiguration = new MappingConfiguration(rootScope, contextFactory, new NullObjectConverter());
            return mappingConfiguration;
        }
    }
}