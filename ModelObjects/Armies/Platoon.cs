using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace ModelObjects.Armies
{
    public class Platoon : ModelBase
    {
        public string Code { get; set; } = string.Empty;
        public List<Member> Members { get; set; } = new List<Member>();
        public string LeaderReference { get; set; } = string.Empty;
    }
}