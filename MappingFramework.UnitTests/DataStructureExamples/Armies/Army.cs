using MappingFramework.Languages.DataStructure;

namespace MappingFramework.UnitTests.DataStructureExamples.Armies
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