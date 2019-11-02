using AdaptableMapper.Model.Language;

namespace ModelObjects.Hardwares
{
    public class Memory : ModelBase
    {
        public Memory()
        {
            MemoryChips = new ModelList<MemoryChip>(this);
        }

        public ModelList<MemoryChip> MemoryChips { get; set; }
        public string Brand { get; set; } = string.Empty;
    }
}