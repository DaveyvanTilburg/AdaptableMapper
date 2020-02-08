using MappingFramework.Model;

namespace ModelObjects.Armies
{
    public class Member : ModelBase
    {
        public Member()
        {
            CrewMembers = new ModelList<CrewMember>(this);
        }

        public string Name { get; set; } = string.Empty;
        public ModelList<CrewMember> CrewMembers { get; set; }
    }
}