using MappingFramework.Languages.DataStructure;

namespace MappingFramework.UnitTests.DataStructureExamples.Hardwares
{
    public class Root : TraversableDataStructure
    {
        public Root()
        {
            Motherboards = new ChildList<Motherboard>(this);
        }

        public ChildList<Motherboard> Motherboards { get; set; }
    }
}