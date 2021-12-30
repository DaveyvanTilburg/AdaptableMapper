using System.Collections.Generic;

namespace MappingFramework.UnitTests.DataStructureExamples.Simple
{
    public class NoItem
    {
        public List<NoItem> NoItems { get; set; } = new();
        public string Code { get; set; } = string.Empty;
    }
}