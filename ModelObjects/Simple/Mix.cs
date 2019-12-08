using System.Collections.Generic;
using AdaptableMapper.Model;

namespace ModelObjects.Simple
{
    public class Mix : ModelBase
    {
        public Mix()
        {
            Items = new ModelList<Item>(this);
        }

        public List<NoItem> NoItems { get; set; } = new List<NoItem>();
        public ModelList<Item> Items { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}