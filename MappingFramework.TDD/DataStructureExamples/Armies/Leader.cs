using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Armies
{
    public class Leader : TraversableDataStructure
    {
        public string Reference { get; set; } = string.Empty;
        public LeaderPerson LeaderPerson { get; set; } = new LeaderPerson();
    }
}