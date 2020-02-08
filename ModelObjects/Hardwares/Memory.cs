using MappingFramework.Model;

namespace ModelObjects.Hardwares
{
    public class Memory : ModelBase
    {
        public Memory()
        {
            MemoryChips = new ModelList<MemoryChip>(this);
        }

        public ModelList<MemoryChip> MemoryChips { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}