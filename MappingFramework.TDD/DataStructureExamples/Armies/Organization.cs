using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Armies
{
    public class Organization : TraversableDataStructure
    {
        public Organization()
        {
            Leaders = new ChildList<Leader>(this);
        }

        public ChildList<Leader> Leaders { get; set; }
    }
}