using System.Collections.Generic;
using AdaptableMapper.Model.Language;

namespace ModelObjects.Simple
{
    public class Mix : ModelBase
    {
        public List<NoItem> NoItems { get; set; } = new List<NoItem>();
    }
}