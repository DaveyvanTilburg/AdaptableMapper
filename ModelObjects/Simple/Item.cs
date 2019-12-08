using AdaptableMapper.Model;

namespace ModelObjects.Simple
{
    public class Item : ModelBase
    {
        public Item()
        {
            Items = new ModelList<Item>(this);
        }

        public ModelList<Item> Items { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}