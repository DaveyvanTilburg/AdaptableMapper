using AdaptableMapper.Model;

namespace ModelObjects.Hardwares
{
    public class CPU : ModelBase
    {
        public string Brand { get; set; } = string.Empty;
        public string Cores { get; set; } = string.Empty;
        public string Speed { get; set; } = string.Empty;
    }
}