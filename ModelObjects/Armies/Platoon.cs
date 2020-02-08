using MappingFramework.Model;

namespace ModelObjects.Armies
{
    public class Platoon : ModelBase
    {
        public Platoon()
        {
            Members = new ModelList<Member>(this);
        }

        public string Code { get; set; } = string.Empty;
        public string Deployed { get; set; } = string.Empty;
        public ModelList<Member> Members { get; set; }
        public string LeaderReference { get; set; } = string.Empty;
    }
}