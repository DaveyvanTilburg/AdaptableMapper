using AdaptableMapper.Model;

namespace ModelObjects.Armies
{
    public class Leader : ModelBase
    {
        public string Reference { get; set; } = string.Empty;
        public LeaderPerson LeaderPerson { get; set; } = new LeaderPerson();
    }
}