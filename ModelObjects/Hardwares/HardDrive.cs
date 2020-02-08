using MappingFramework.Model;

namespace ModelObjects.Hardwares
{
    public class HardDrive : ModelBase
    {
        public string Size { get; set; } = string.Empty;
        public string Speed { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
    }
}