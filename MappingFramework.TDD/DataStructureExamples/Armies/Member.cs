using MappingFramework.Languages.DataStructure;

namespace MappingFramework.TDD.DataStructureExamples.Armies
{
    public class Member : TraversableDataStructure
    {
        public Member()
        {
            CrewMembers = new ChildList<CrewMember>(this);
        }

        public string Name { get; set; } = string.Empty;
        public ChildList<CrewMember> CrewMembers { get; set; }
    }
}