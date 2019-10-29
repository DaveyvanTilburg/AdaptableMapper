using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace AdaptableObjects
{
    public class Member : ModelBase
    {
        public string Name { get; set; } = string.Empty;
        public List<CrewMember> CrewMembers { get; set; } = new List<CrewMember>();
    }
}