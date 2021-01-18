using MappingFramework.DataStructure;

namespace MappingFramework.TDD.DataStructureExamples.Armies
{
    public class Platoon : TraversableDataStructure
    {
        public Platoon()
        {
            Members = new ChildList<Member>(this);
        }

        public string Code { get; set; } = string.Empty;
        public string Deployed { get; set; } = string.Empty;
        public ChildList<Member> Members { get; set; }
        public string LeaderReference { get; set; } = string.Empty;
    }
}