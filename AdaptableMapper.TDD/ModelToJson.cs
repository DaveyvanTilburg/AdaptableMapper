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
            JToken xExpectedResult = JToken.Parse(expectedResult);

            errorObserver.GetRaisedWarnings().Count.Should().Be(0);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
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

            var memoryChip5 = new MemoryChip { Size = "512", Brand = "Intel" };
            var memoryChip6 = new MemoryChip { Size = "512", Brand = "Intel" };
            var memory1 = new Memory { Brand = "HyperX" };
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
            var memoryChip1 = new MemoryChip { Size = "512", Brand = "Biostar" };
            var memoryChip2 = new MemoryChip { Size = "512", Brand = "Biostar" };
            var memoryChip3 = new MemoryChip { Size = "512", Brand = "Biostar" };
            var memoryChip4 = new MemoryChip { Size = "512", Brand = "Biostar" };
            var motherboard1graphicalcard1 = new GraphicalCard();
            motherboard1graphicalcard1.CPUs.Add(motherboard1graphicalcard1cpu1);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip1);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip2);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip3);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip4);

            var motherboard1 = new Motherboard { Brand = "MSI" };
            motherboard1.CPUs.Add(motherboard1cpu1);
            motherboard1.GraphicalCards.Add(motherboard1graphicalcard1);
            motherboard1.HardDrives.Add(harddrive1);

            return motherboard1;
        }

        private static Motherboard CreateMotherboard2()
        {
            var harddrive1 = new HardDrive { Brand = "BData", Size = "262144", Speed = "Rotating" };

            var memoryChip5 = new MemoryChip { Size = "2048" };
            var memory1 = new Memory { Brand = "Corsair" };
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
            var memoryChip1 = new MemoryChip { Size = "512", Brand = "Biostar" };
            var memoryChip2 = new MemoryChip { Size = "512", Brand = "Biostar" };
            var motherboard1graphicalcard1 = new GraphicalCard();
            motherboard1graphicalcard1.CPUs.Add(motherboard1graphicalcard1cpu1);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip1);
            motherboard1graphicalcard1.MemoryChips.Add(memoryChip2);

            var motherboard1 = new Motherboard { Brand = "Biostar" };
            motherboard1.CPUs.Add(motherboard1cpu1);
            motherboard1.GraphicalCards.Add(motherboard1graphicalcard1);
            motherboard1.HardDrives.Add(harddrive1);

            return motherboard1;
        }

        private static MappingConfiguration GetFakedMappingConfiguration()
        {
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

            var HardDrivesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    hardDriveBrand,
                    hardDriveSize,
                    hardDriveSpeed
                },
                new Model.ModelGetScope("Motherboards/HardDrives"),
                new Json.JsonTraversalThis(),
                new Json.JsonTraversalTemplate("AvailableHardDrives[0]"),
                new Json.JsonChildCreator()
            );

            var rootScope = new MappingScopeRoot(
                new List<MappingScopeComposite>
                {
                    HardDrivesScope
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