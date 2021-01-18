using System.Collections.Generic;
using MappingFramework.DataStructure;

namespace MappingFramework.TDD.DataStructureExamples.Simple
{
    public class Mix : TraversableDataStructure
    {
        public Mix()
        {
            Items = new ChildList<Item>(this);
        }

        public List<NoItem> NoItems { get; set; } = new List<NoItem>();
        public ChildList<Item> Items { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}