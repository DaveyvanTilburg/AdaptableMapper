using MappingFramework.Model;

namespace ModelObjects.Hardwares
{
    public class GraphicalCard : ModelBase
    {
        public GraphicalCard()
        {
            CPUs = new ModelList<CPU>(this);
            MemoryChips = new ModelList<MemoryChip>(this);
        }

        public ModelList<CPU> CPUs { get; set; }
        public ModelList<MemoryChip> MemoryChips { get; set; }
        public string Brand { get; set; }
    }
}