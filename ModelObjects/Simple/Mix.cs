using System.Collections.Generic;
using AdaptableMapper.Model;

namespace ModelObjects.Simple
{
    public class Mix : ModelBase
    {
        public List<NoItem> NoItems { get; set; } = new List<NoItem>();
    }
}