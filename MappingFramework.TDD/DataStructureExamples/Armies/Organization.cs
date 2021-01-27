using MappingFramework.Languages.DataStructure;

namespace MappingFramework.TDD.DataStructureExamples.Armies
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