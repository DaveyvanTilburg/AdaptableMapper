using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace ModelObjects.Hardwares
{
    public class Motherboard : ModelBase
    {
        public List<CPU> CPUs { get; set; } = new List<CPU>();
        public List<GraphicalCard> GraphicalCards { get; set; } = new List<GraphicalCard>();
        public List<Memory> Memories { get; set; } = new List<Memory>();
        public List<HardDrive> HardDrives { get; set; } = new List<HardDrive>();
        public string Brand { get; set; } = string.Empty;
    }
}