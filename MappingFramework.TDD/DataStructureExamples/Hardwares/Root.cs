using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Hardwares
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