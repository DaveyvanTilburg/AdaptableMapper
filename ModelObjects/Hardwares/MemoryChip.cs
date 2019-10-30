using AdaptableMapper.Model.Language;

namespace ModelObjects.Hardwares
{
    public class MemoryChip : ModelBase
    {
        public string Size { get; set; }
        public string Speed { get; set; }
        public string Brand { get; set; } = string.Empty;
    }
}