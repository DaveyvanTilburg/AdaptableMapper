using System.Collections.Generic;
using AdaptableMapper.Model.Language;

namespace ModelObjects.Simple
{
    public class Item : ModelBase
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public string Code { get; set; } = string.Empty;
    }
}