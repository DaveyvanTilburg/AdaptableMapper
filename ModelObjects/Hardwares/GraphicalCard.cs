using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace ModelObjects.Hardwares
{
    public class GraphicalCard : ModelBase
    {
        public List<CPU> CPUs { get; set; } = new List<CPU>();
        public List<MemoryChip> MemoryChips { get; set; } = new List<MemoryChip>();
        public string Brand { get; set; }
    }
}