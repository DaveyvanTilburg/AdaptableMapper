using AdaptableMapper.Model.Language;

namespace ModelObjects.Hardwares
{
    public class Motherboard : ModelBase
    {
        public Motherboard()
        {
            GraphicalCards = new ModelList<GraphicalCard>(this);
            Memories = new ModelList<Memory>(this);
            HardDrives = new ModelList<HardDrive>(this);
        }

        public CPU CPU { get; set; } = new CPU();
        public ModelList<GraphicalCard> GraphicalCards { get; set; }
        public ModelList<Memory> Memories { get; set; }
        public ModelList<HardDrive> HardDrives { get; set; }
        public string Brand { get; set; } = string.Empty;
    }
}