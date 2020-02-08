using MappingFramework.Model;

namespace ModelObjects.Armies
{
    public class LeaderPerson : ModelBase
    {
        public Person Person { get; set; } = new Person();
    }
}