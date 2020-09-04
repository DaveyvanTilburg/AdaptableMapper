using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Armies
{
    public class LeaderPerson : TraversableDataStructure
    {
        public Person Person { get; set; } = new Person();
    }
}