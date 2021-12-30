using System.Collections.Generic;
using MappingFramework.Languages.DataStructure;

namespace MappingFramework.UnitTests.DataStructureExamples.Simple
{
    public class Mix : TraversableDataStructure
    {
        public Mix()
        {
            Items = new ChildList<Item>(this);
        }

        public List<NoItem> NoItems { get; set; } = new();
        public ChildList<Item> Items { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}