using MappingFramework.Languages.DataStructure;

namespace MappingFramework.TDD.DataStructureExamples.Hardwares
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