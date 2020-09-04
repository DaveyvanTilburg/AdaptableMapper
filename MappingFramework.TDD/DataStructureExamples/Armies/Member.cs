using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Armies
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