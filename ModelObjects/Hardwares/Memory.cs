using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace ModelObjects.Hardwares
{
    public class Memory : ModelBase
    {
        public List<MemoryChip> MemoryChips { get; set; } = new List<MemoryChip>();
        public string Brand { get; set; } = string.Empty;
    }
}