using MappingFramework.Languages.DataStructure;

namespace MappingFramework.TDD.DataStructureExamples.Armies
{
    public class LeaderPerson : TraversableDataStructure
    {
        public Person Person { get; set; } = new Person();
    }
}