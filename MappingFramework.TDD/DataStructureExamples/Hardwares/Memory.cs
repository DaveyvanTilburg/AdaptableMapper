using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Hardwares
{
    public class Memory : TraversableDataStructure
    {
        public Memory()
        {
            MemoryChips = new ChildList<MemoryChip>(this);
        }

        public ChildList<MemoryChip> MemoryChips { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}