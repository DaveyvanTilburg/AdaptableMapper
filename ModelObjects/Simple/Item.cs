using System.Collections.Generic;
using AdaptableMapper.Model;

namespace ModelObjects.Simple
{
    public class Item : ModelBase
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}