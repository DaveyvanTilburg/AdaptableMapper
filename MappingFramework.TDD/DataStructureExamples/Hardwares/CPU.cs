using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Hardwares
{
    public class CPU : TraversableDataStructure
    {
        public string Brand { get; set; } = string.Empty;
        public string Cores { get; set; } = string.Empty;
        public string Speed { get; set; } = string.Empty;
    }
}