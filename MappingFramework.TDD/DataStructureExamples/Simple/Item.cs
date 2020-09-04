using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Simple
{
    public class Item : TraversableDataStructure
    {
        public Item()
        {
            Items = new ChildList<Item>(this);
        }

        public ChildList<Item> Items { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}