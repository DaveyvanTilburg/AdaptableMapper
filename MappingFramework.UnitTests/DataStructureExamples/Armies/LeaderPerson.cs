using MappingFramework.Languages.DataStructure;

namespace MappingFramework.UnitTests.DataStructureExamples.Armies
{
    public class LeaderPerson : TraversableDataStructure
    {
        public Person Person { get; set; } = new Person();
    }
}