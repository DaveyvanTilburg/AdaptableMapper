using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Hardwares
{
    public class HardDrive : TraversableDataStructure
    {
        public string Size { get; set; } = string.Empty;
        public string Speed { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
    }
}