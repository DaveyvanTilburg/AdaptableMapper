using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Armies
{
    public class Army : TraversableDataStructure
    {
        public Army()
        {
            Platoons = new ChildList<Platoon>(this);
        }

        public ChildList<Platoon> Platoons { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}